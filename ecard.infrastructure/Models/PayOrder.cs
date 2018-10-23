using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class PayOrder
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        public int userId { get; set; }
        /// <summary>
        /// 订单类型 1 会员升级 
        /// </summary>
        public int orderType { get; set; }
        /// <summary>
        /// 1 未付款 2已付款 3已取消
        /// </summary>
        public int orderState { get; set; }
        /// <summary>
        /// 项目 member--购买会员 shopowner购买店长 shopkeeper购买店主
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payAmount { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? payTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 是否返利
        /// </summary>
        public bool IsRebate { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }

    public class PayOrderStates
    {
        /// <summary>
        /// 请选择
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 等待付款
        /// </summary>
        public const int awaitPay = 1;
        /// <summary>
        /// 已付款
        /// </summary>
        public const int paid = 2;
        /// <summary>
        /// 取消
        /// </summary>
        public const int cancel = 3;
    }

    public class PayOrderTypes
    {
        /// <summary>
        /// 会员升级
        /// </summary>
        public const int MmeberUp = 1;
    }

    public class PayOrderItems
    {
        /// <summary>
        ///  member--购买会员 shopowner购买店长 shopkeeper购买店主
        /// </summary>
        public const string member = "member";
        /// <summary>
        /// 购买店长
        /// </summary>
        public const string shopowner = "shopowner";
        /// <summary>
        /// 购买店主
        /// </summary>
        public const string shopkeeper = "shopkeeper";
    }
}
