using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class CommoditySales : IRecordVersion
    {
        [Key]
        public int commoditySalesId { get; set; }
        /// <summary>
        /// 订单详情id
        /// </summary>
        public int orderDetailId { get; set; }
        /// <summary>
        /// 一级返佣
        /// </summary>
        public decimal oneRebate { get; set; }
        /// <summary>
        /// 二级返佣
        /// </summary>
        public decimal twoRebate { get; set; }
        /// <summary>
        /// 三级返佣
        /// </summary>
        public decimal threeRebate { get; set; }
        /// <summary>
        ///状态 1正常 2撤销
        /// </summary>
        [Bounded(typeof(CommoditySalesState))]
        public int state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime submitTime { get; set; }
        public int RecordVersion { get; set; }
    }

    public class CommoditySalesState
    {
        
        /// <summary>
        /// 正常
        /// </summary>
        public const int normal = 1;
        /// <summary>
        /// 撤销
        /// </summary>
        public const int cancel = 2;
    }
}
