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
    public class ListOrders : EcardModelListRequest<ListOrder>
    {

        public ListOrders() 
        {
            OrderBy = "submitTime Desc";
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        private Bounded _orderState;

        public Bounded orderState
        {
            get
            {
                if (_orderState == null)
                    _orderState = Bounded.Create<Order>("orderState", OrderStates.paid);
                return _orderState;
            }
            set { _orderState = value; }
        }
        /// <summary>
        /// 支付状态
        /// </summary>
        private Bounded _payState;
        public Bounded payState
        {
            get
            {
                if (_payState == null)
                    _payState = Bounded.Create<Order>("payState", PayStates.all);
                return _payState;
            }
            set { _payState = value; }
        }
        /// <summary>
        /// 配送状态
        /// </summary>
        private Bounded _distributionstate;

        public Bounded distributionstate
        {
            get
            {
                if (_distributionstate == null)
                    _distributionstate = Bounded.Create<Order>("distributionstate", DistributionStates.all);
                return _distributionstate;
            }
            set { _distributionstate = value; }
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
            request.orderState = OrderStates.paid;
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
