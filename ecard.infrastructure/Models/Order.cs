using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class Order:IRecordVersion
    {
        [Key]
        public int orderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal freight{get;set;}
        /// <summary>
        /// 总积分，用来计算返利使用
        /// </summary>
        public decimal point { get; set; }
        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal payAmount { get; set; }
        /// <summary>
        /// 订单状态，1等待付款 2已付款 3已发货 4 完成, 5申请退款 6退款
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
        public string  ExpressCompany { get; set; }
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
        /// <summary>
        /// 收货人
        /// </summary>
        public string recipients { get; set; }
        public string zipCode { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string moblie { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
        /// <summary>
        /// 支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 用户地址id
        /// </summary>
        public int UserAddressId { get; set; }
        /// <summary>
        /// 扣除赠送积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 扣除的可提现积分
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 订单完成时间
        /// </summary>
        public DateTime? completeTime { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? ShipTime { get; set; }
        /// <summary>
        /// 是否返佣
        /// </summary>
        public bool IsRebate { get; set; }
        /// <summary>
        /// 是否首单
        /// </summary>
        public bool IsFirs { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public int orderType { get; set; }

        [Timestamp]
        public int RecordVersion { get; set; }
        
    }
    public class DistributionWay
    {
        /// <summary>
        /// 请选择
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 快递
        /// </summary>
        public const int kuaidi = 1;
        /// <summary>
        /// EMS
        /// </summary>
        public const int ems = 2;
        /// <summary>
        /// 平邮
        /// </summary>
        public const int pingyou = 3;
        /// <summary>
        /// 买家承担运费
        /// </summary>
        public const int mjcdyf = 4;
    }
    /// <summary>
    /// 配送状态
    /// </summary>
    public class DistributionStates
    {
        /// <summary>
        /// 请选择
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 未发货
        /// </summary>
        public const int unfilled = 1;
        /// <summary>
        /// 已发货
        /// </summary>
        public const int shipped = 2;
        /// <summary>
        /// 已收货
        /// </summary>
        public const int paid = 3;
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public class OrderStates
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
        /// 已发货
        /// </summary>
        public const int shipped = 3;
        /// <summary>
        /// 完成
        /// </summary>
        public const int complete = 4;
        /// <summary>
        /// 申请退款
        /// </summary>
        public const int applyRefund = 5;
        /// <summary>
        /// 退款
        /// </summary>
        public const int refund = 6;
        /// <summary>
        /// 取消
        /// </summary>
        public const int cancel = 7;

        /// <summary>
        /// 已评价
        /// </summary>
        public const int completePJ =8;

        public static string GetName(int status)
        {
            switch (status)
            {
                case all:
                    return "全部";
                case awaitPay:
                    return "等待付款";
                case paid:
                    return "已付款";
                case shipped:
                    return "已发货";
                case complete:
                    return "已完成";
                case applyRefund:
                    return "申请退款";
                case refund:
                    return "退款";
                case cancel:
                    return "取消";
                default:
                    return " ";
            }


        }
    }
    /// <summary>
    /// 支付状态
    /// </summary>
    public class PayStates
    {
        /// <summary>
        /// 请选择
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 未支付
        /// </summary>
        public const int non_payment = 1;
        /// <summary>
        /// 已付款
        /// </summary>
        public const int paid = 2;
    }
    /// <summary>
    /// 支付方式
    /// </summary>
    public class PayTypes
    {
        /// <summary>
        /// 请选择
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 微信支付
        /// </summary>
        public const int weChatPayment = 1;
        /// <summary>
        /// 货到付款
        /// </summary>
        public const int cashOnDelivery = 2;
        /// <summary>
        ///  支付宝
        /// </summary>
        public const int Alipay = 3;
    }


    public class OrderType
    {
        /// <summary>
        /// 普通订单
        /// </summary>
        public const int normal = 1;
        /// <summary>
        /// 秒杀订单
        /// </summary>
        public const int secondKill = 2;
    }
    public class UserOrderDetail
    {
        public Order item { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
