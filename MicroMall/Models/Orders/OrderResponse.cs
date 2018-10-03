using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class OrderResponse
    {
        /// <summary>
        /// 上一页
        /// </summary>
        public int PrePage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        ///// <summary>
        ///// 待付款上一页
        ///// </summary>
        //public int StayPayPrePage { get; set; }
        ///// <summary>
        ///// 待付款下一页
        ///// </summary>
        //public int StayPayNextPage { get; set; }

        /// <summary>
        /// 全部订单
        /// </summary>
        public List<ListOrder> List { get; set; }
        /// <summary>
        /// 待付款订单
        /// </summary>
        public List<ListOrder> StayPaymentList { get; set; }
    }
}