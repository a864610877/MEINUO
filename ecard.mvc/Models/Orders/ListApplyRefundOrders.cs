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

namespace Ecard.Mvc.Models.Orders
{
    public class ListApplyRefundOrders : EcardModelListRequest<ListOrder>
    {
        public ListApplyRefundOrders() 
        {
            OrderBy = "submitTime Desc";
        }
        
        

        [Dependency]
        [NoRender]
        public IOrderService orderService { get; set; }

        [Dependency]
        [NoRender]
        public IOrderDetailService orderDetailService { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListOrder item)
        {
            yield return new ActionMethodDescriptor("OrderDetail", null, new { id = item.orderId });
        }

        public void Query() 
        {
            var request = new OrderRequest();
            request.orderState = OrderStates.applyRefund;
            var query = orderService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListOrder(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListOrder>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<ListOrder> AjaxQuery(OrderRequest request)
        {
            var data = new List<ListOrder>();
            request.orderState = OrderStates.applyRefund;
            var query = orderService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListOrder(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('OrderDetail','/Order/OrderDetail/" + item.orderId + "') class='tablelink'>订单详情 &nbsp;</a> ";
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
