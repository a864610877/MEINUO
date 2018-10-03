using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class fz_Rebate : IRecordVersion
    {
        [Key]
        public int rebateId { get; set; }
        /// <summary>
        /// 订单详情id
        /// </summary>
        public int orderDetailId { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public int accountId { get; set; }
        /// <summary>
        /// 实际返利积分
        /// </summary>
        public decimal payAmount { get; set; }
        /// <summary>
        /// 返利比例
        /// </summary>
        public decimal reateRatio { get; set; }
        /// <summary>
        /// 返利积分
        /// </summary>
        public decimal reateAmount { get; set; }
        /// <summary>
        /// 类型 1一级 2二级 3三级
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 状态 1正常 2撤销
        /// </summary>
        public int state { get; set; }

        public DateTime submitTime { get; set; }
        public int RecordVersion { get; set; }
    }

    public class RebateState
    {
        public const int normal = 1;
        public const int cancel = 2;
    }

    public class RebateType
    {
        public const int zero = 0;
        public const int one = 1;
        public const int two = 2;
        public const int three = 3;
    }

    public class RebateLog
    {
        public int rebateId { get; set; }
        public decimal reateAmount { get; set; }

        public string DisplayName { get; set; }

        public int type { get; set; }

        public DateTime submitTime { get; set; }
    }
}
