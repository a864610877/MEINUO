using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class OrderRequest : PageRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string  orderNo { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int? orderState { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public int? payState { get; set; }
        /// <summary>
        /// 配送状态
        /// </summary>
        public int? distributionstate { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startSubmitTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endSubmitTime { get; set; }
    }

    public class MicroMallOrderRequest:PageRequest
    {
        public int UserId { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int? orderState { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int? OrderType { get; set; }
    }
}
