using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ShoppingCarts
{
    public class ListShoppingCarts : LayoutModel
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public int PrevPage { get; set; }        
        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage { get; set; }

        public List<ListShoppingCart> List { get; set; }
        [Dependency]
        public IShoppingCartService ShoppingCartService { get; set; }

        public void Query()
        {
            Load();
            if(UserInformation!=null)
            {
                var request = new ShoppingCartRequest();
                request.UserId = UserInformation.UserId;
                var query = ShoppingCartService.GetByUserId(request);
                if(query!=null)
                {
                    PageIndex = request.PageIndex;
                    List = query.ModelList.Select(x => new ListShoppingCart(x)).ToList();
                    foreach(var item in List)
                    {
                       if(!string.IsNullOrWhiteSpace(item.images))
                       {
                           string[] sp = item.images.Split(',');
                           if(sp.Count()>0)
                           {
                               item.ImageUrl = ImageUrl + "/CommodityImages/" + sp[0];
                           }
                       }
                    }
                    Page(request.PageIndex, request.PageSize, query.TotalCount);
                }
            }
        }

        public void Page(int pageIndex,int pageSize,int total)
        {
            PageIndex = pageIndex;
            int pageToTal =total / pageSize;
            int more = total % pageSize;
            if (more > 0)
                pageToTal += 1;
            if (pageIndex == pageToTal)
            {
                NextPage = 0;
                int prve = pageIndex - 1;
                if (prve <= 0)
                    PrevPage = 0;
                else
                {
                    PrevPage = prve;
                }
            }
            else if (pageIndex < pageToTal)
            {
                NextPage += 1;
                int prve = pageIndex - 1;
                if (prve <= 0)
                    PrevPage = 0;
                else
                {
                    PrevPage = prve;
                }
            }
            else
            {
                PrevPage = 0;
                NextPage = 0;
            }
        }

        public ResultMessage Delete(int id)
        {
            try
            {
                var item = ShoppingCartService.GetById(id);
                if (item != null)
                {
                    ShoppingCartService.Delete(item);
                }
                return new ResultMessage() { Code = 0 };
            }
            catch(Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1,Msg="系统异常" };
            }
        }
    }
}