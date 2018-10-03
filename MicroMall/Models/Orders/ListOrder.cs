using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class ListOrder
    {
        public int orderId { get; set; }
        public string orderNo { get; set; }
        public string ImageUrl { get; set; }

        public int commodityId { get; set; }
        public string commodityName { get; set; }
        public int Num { get; set; }
        /// <summary>
        /// 实际付款金额
        /// </summary>
        public decimal Amount { get; set;}
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight { get; set; }

        
        /// <summary>
        /// 订单状态
        /// </summary>
        public int orderState { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public int payState { get; set; }
        public DateTime submitTime { get; set; }



        public List<CommodityDetail> ListCommodityDetail { get; set; }
    }

    public class CommodityDetail
    {
        public int commodityId { get; set; }
        public string commodityName { get; set; }
        public decimal price { get; set; }

        public int quantity { get; set; }

        public string image { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string specification { get; set; }
    }
}