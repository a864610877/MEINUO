using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Infrastructure
{
    public class AccountModel
    {

        public int accountId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 推荐会员
        /// </summary>
        public string salerName { get; set; }
        /// <summary>
        /// 会员推荐码
        /// </summary>
        public string orangeKey { get; set; }
        /// <summary>
        /// 会员推荐链接
        /// </summary>
        public string qrCodeUrl { get; set; }
        /// <summary>
        /// 会员推荐二维码
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 可提现积分
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal amount { get; set; }

        public DateTime submitTime { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
         [Bounded(typeof(AccountGrade))]
        public int grade { get; set; }
    }
}
