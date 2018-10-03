using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    public class OrderDetail
    {
        [Key]
        public int orderDetailId { get; set; }
        public int orderId { get; set; }

        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int commodityId { get; set; }

        public string commodityName { get; set; }
        /// <summary>
        /// 规格描述
        /// </summary>
        public string specification { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal amount { get;set; }
        /// <summary>
        /// 用来返利得积分
        /// </summary>
        public decimal point { get; set; }
        /// <summary>
        /// 是否返佣
        /// </summary>
        public bool IsRebate { get; set; }
    }


    public class OrderDetailView
    {
        public int orderId { get; set; }

        public decimal amount { get; set; }

        public int point { get; set; }

        public string orderNo { get; set; }

        public DateTime submitTime { get; set; }

        public string images { get; set; }

        public string commodityName { get; set; }

        public int quantity { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public int commodityId { get; set; }
        public decimal price { get; set; }

        public string specification { get; set; }
    }
}
