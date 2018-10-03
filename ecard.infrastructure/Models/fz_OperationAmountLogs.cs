using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 余额操作明细 -- Matthew 20170419
    /// </summary>
    public class fz_OperationAmountLogs : IRecordVersion
    {

        /// <summary>
        /// 主键 自增
        /// </summary>
        [Key]
        public int perationAmountLogId { get; set; }

        /// <summary>
        /// accountId
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 积分
        /// </summary>

        public decimal amount { get; set; }

        /// <summary>
        /// 类型 1收入 2支出
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public int category { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime submitTime { get; set; }

        [Timestamp]
        public int RecordVersion { get; set; }
    }

    public class OperationAmountType
    {
        /// <summary>
        /// 收入
        /// </summary>
        public const int Income = 1;
        /// <summary>
        /// 支出
        /// </summary>
        public const int expenditure = 2;

        public static string GetByName(int value)
        {
            switch (value)
            {
                case Income:
                    return "收入";
                case expenditure:
                    return "支出";
                default:
                    return "";
            }
        } 
    }

    public class OperationAmountCategory
    {
        /// <summary>
        /// 返佣
        /// </summary>
        public const int rebate = 1;
        /// <summary>
        /// 提现
        /// </summary>
        public const int withdraw = 2;
        /// <summary>
        /// 撤销返佣
        /// </summary>
        public const int cancelRebate = 3;
        /// <summary>
        /// 撤销提现
        /// </summary>
        public const int cancelwithdraw = 4;

        public static string GetByName(int value)
        {
            switch (value)
            { 
                case rebate:
                    return "返佣";
                case withdraw:
                    return "提现";
                case cancelRebate:
                    return "撤销返佣";
                case cancelwithdraw:
                    return "撤销提现";
                default:
                    return "";
            }
        }
    }
}
