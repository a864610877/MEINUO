using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class Withdraw
    {
        [Key]
        public int withdrawId { get; set; }
        /// <summary>
        /// 提现用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 提现积分
        /// </summary>
        public decimal point { get; set; }
        /// <summary>
        /// 换算后金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 审核员
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Bounded(typeof(WithdrawStates))]
        public int state { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
        /// <summary>
        /// 用户openid
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
    }

    public class WithdrawStates
    {
        /// <summary>
        /// 全部
        /// </summary>
        public const int All = 0;
        /// <summary>
        /// 待审核
        /// </summary>
        public const int notaudit = 1;
        /// <summary>
        /// 成功
        /// </summary>
        public const int success = 2;
        /// <summary>
        /// 失败
        /// </summary>
        public const int failure = 3;

        public static string GetName(int status)
        {
            switch (status)
            {
                case All:
                    return "全部";
                case notaudit:
                    return "待审核";
                case success:
                    return "提现成功";
                case failure:
                    return "提现失败";
                default:
                    return "";
            }
        
        
        }
    }
}
