using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Orders
{
    public class ListOrderDetail
    {

        private readonly OrderDetail _orderDetail;
        [NoRender]
        public OrderDetail orderDetail
        {
            get { return _orderDetail; }
        }

        public ListOrderDetail()
        {
            _orderDetail = new OrderDetail();
        }

        public ListOrderDetail(OrderDetail orderdetail)
        {
            _orderDetail = orderdetail;
        }
        [NoRender]
        public string orderNo { get { return orderDetail.orderNo; } }
        [NoRender]
        public int orderDetailId { get { return orderDetail.orderDetailId; } }
        public int commodityId { get { return orderDetail.commodityId; } }
        public string commodityName { get { return orderDetail.commodityName; } }
        public decimal price { get { return orderDetail.price; } }
        public int quantity { get { return orderDetail.quantity; } }


        [NoRender]
        public string boor { get; set; }
    }
}
