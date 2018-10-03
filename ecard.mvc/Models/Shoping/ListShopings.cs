using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Shoping
{
    public class ListShopings:EcardModelListRequest<ListShoping>
    {

        public ListShopings()
        {
            OrderBy = "SubmitTime desc";
        }
        public string commodityName { get; set; }

        public string commodityNo { get; set; }

        public string commdityKeyword { get; set; }

        //public DateTime? startTime { get; set; }

        //public DateTime? endTime { get; set; }

        private Bounded _commodityState;

        public Bounded commodityState
        {
            get
            {
                if (_commodityState == null)
                    _commodityState = Bounded.Create<Commodity>("commodityState", CommodityStates.all);
                return _commodityState;
            }
            set { _commodityState = value; }
        }

        [Dependency]
        [NoRender]
        public ICommodityService commodityService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListShoping item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.commodityId });
            yield return new ActionMethodDescriptor("DeleteShoping", null, new { id = item.commodityId });
        }

        public void Query()
        {
            var request = new CommodityRequest();
            var query = commodityService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListShoping(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListShoping>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<ListShoping> AjaxQuery(CommodityRequest request)
        {
            var data = new List<ListShoping>();
            var query = commodityService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListShoping(x)).ToList();
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("commodityedit"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/Shoping/Edit/" + item.commodityId + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("commoditydelete"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('DeleteShoping','/Shoping/DeleteShoping/" + item.commodityId + "') class='tablelink'>删除 </a> ";
                    }
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
}
