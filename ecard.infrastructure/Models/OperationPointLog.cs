using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 积分操作记录
    /// </summary>
    public class OperationPointLog
    {
        [Key]
        public int operationPointLogId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal point { get; set; }
        /// <summary>
        /// 操作账户(presentExp赠送积分，notActivatePoint未激活积分，activatePoint激活积分)
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }

    public class OperationPointLogTypes
    {
        /// <summary>
        /// presentExp赠送积分
        /// </summary>
        public const string presentExp = "presentExp";
        /// <summary>
        /// 未激活积分
        /// </summary>
        public const string notActivatePoint = "notActivatePoint";
        /// <summary>
        /// 赠送积分
        /// </summary>
        public const string activatePoint = "activatePoint";
    }
}
