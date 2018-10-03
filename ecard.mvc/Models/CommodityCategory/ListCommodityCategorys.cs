using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.CommodityCategory
{
    public class ListCommodityCategorys : EcardModelListRequest<ListCommodityCategory>
    {
        [Dependency]
        [NoRender]
        public ICommodityCategorysService ICommodityCategorysService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListCommodityCategory item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.commodityCategoryId });
            //yield return new ActionMethodDescriptor("Delete", null, new { id = item.commodityCategoryId });
        }
        public string name { get; set; }
        public void Query()
        {
            var request = new CommodityCategorysRequest();
            var query = ICommodityCategorysService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListCommodityCategory(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListCommodityCategory>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }
        public List<ListCommodityCategory> AjaxQuery(CommodityCategorysRequest request)
        {

            var data = new List<ListCommodityCategory>();
            var query = ICommodityCategorysService.Query(request);
            if (query != null)
            {
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                data = query.ModelList.Select(x => new ListCommodityCategory(x)).ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("EditCommodityCategory"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/CommodityCategory/Edit/" + item.commodityCategoryId + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    //if (roles[0].IsSuper || roles[0].Permissions.Contains("DeleteCommodityCategory"))
                    //{
                    //    item.boor += "<a href='#' onclick=OperatorThis('Delete','/CommodityCategory/Delete/" + item.commodityCategoryId + "') class='tablelink'>删除 </a> ";
                    //}
                }
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);

            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            return data;
        }
    }

    public class ListCommodityCategory
    {

        public ListCommodityCategory(fz_CommodityCategorys item)
        {
          commodityCategoryId=item.commodityCategoryId;
            Name=item.name;
        }
        [NoRender]
        public int commodityCategoryId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        [NoRender]
        public string boor{get;set;}
    }
}
