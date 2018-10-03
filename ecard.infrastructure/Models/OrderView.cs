using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Infrastructure.Models
{
    public class OrderView
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        public int  orderId { get; set; }

        public string orderNo { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight { get; set; }
        /// <summary>
        /// 抵扣积分
        /// </summary>
        public decimal point { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal payAmount { get; set; }
        /// <summary>
        /// 订单状态，1等待付款 2已付款 3已发货 4完成, 5申请退款 6退款
        /// </summary>
        [Bounded(typeof(OrderStates))]
        public int orderState { get; set; }
        /// <summary>
        /// 支付状态 1未支付 2已付款
        /// </summary>
        [Bounded(typeof(PayStates))]
        public int payState { get; set; }
        /// <summary>
        /// 配送方式
        /// </summary>
        [Bounded(typeof(DistributionWay))]
        public int distributionType { get; set; }
        /// <summary>
        /// 支付方式 1微信支付 2货到付款
        /// </summary>
        [Bounded(typeof(PayTypes))]
        public int payType { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int provinceId { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int cityId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string detailedAddress { get; set; }
        /// <summary>
        /// 配送状态
        /// </summary>
        [Bounded(typeof(DistributionStates))]
        public int distributionstate { get; set; }


        public string recipients { get; set; }
        public string zipCode { get; set; }
        public string moblie { get; set; }
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
