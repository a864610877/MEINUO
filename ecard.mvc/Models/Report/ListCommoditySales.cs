using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Report
{
    public class ListCommoditySales : EcardModelListRequest<ListCommoditySale>
    {

        [Dependency, NoRender]
        public ICommoditySalesService ICommoditySalesService { get; set; }
        public void Query()
        {
            var request = new CommoditySalesStatisticsRequest();
            var query = ICommoditySalesService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListCommoditySale(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListCommoditySale>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<ListCommoditySale> AjaxQuery(CommoditySalesStatisticsRequest request)
        {

            var data = new List<ListCommoditySale>();
            var query = ICommoditySalesService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListCommoditySale(x)).ToList();
                //foreach (var item in data)
                //{
                //    item.boor += "<a href='#' onclick=OperatorThis('Edit','/Account/Edit/" + item.accountId + "') class='tablelink'>编辑 &nbsp;</a> ";
                //    item.boor += "<a href='#' onclick=OperatorThis('Delete','/Account/Delete/" + item.accountId + "') class='tablelink'>删除 </a> ";
                //}
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);

            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            return data;
        }
    }
    public class ListCommoditySale
    {
        private readonly CommoditySalesStatistics _innerObject;
        [NoRender]
        public CommoditySalesStatistics InnerObject
        {
            get { return _innerObject; }
        }

        public ListCommoditySale()
        {
            _innerObject = new CommoditySalesStatistics();
        }
        public ListCommoditySale(CommoditySalesStatistics innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int commodityId { get { return InnerObject.commodityId; } }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get { return InnerObject.commodityName; } }
        /// <summary>
        /// 总销售数量
        /// </summary>
        public int quantity { get { return InnerObject.quantity; } }
        /// <summary>
        /// 总销售额
        /// </summary>
        public decimal amount { get { return InnerObject.amount; } }

        /// <summary>
        /// 总返佣
        /// </summary>
        public decimal rebateAmount { get { return InnerObject.RebateAmount; } }
    }
}
