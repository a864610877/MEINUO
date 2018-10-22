using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 会员
    /// </summary>
    public class Account : IRecordVersion
    {
        [Key]
        public int accountId { get; set; }
        /// <summary>
        /// 推荐码
        /// </summary>
        public string orangeKey { get; set; }

        /// <summary>
        /// 关联userId
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 推荐人(userId)
        /// </summary>
        public int salerId { get; set; }
        /// <summary>
        /// 赠送总积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 未激活积分（只能用于抵扣）
        /// </summary>
       // public decimal notActivatePoint { get; set; }
        /// <summary>
        /// 激活积分（可用于提现）
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 消费积分汇总
        /// </summary>
       // public decimal payPoint { get; set; }
        /// <summary>
        /// 提现积分汇总
        /// </summary>
      //  public decimal withdrawPoint { get; set; }
        /// <summary>
        /// 状态 1正常 2停用
        /// </summary>
        [Bounded(typeof(AccountStates))]
        public int state { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
        /// <summary>
        /// 获取二维码图片使用
        /// </summary>
        public string ticket { get; set; }
        /// <summary>
        /// 二维码下载地址
        /// </summary>
        public string qrCodeUrl { get; set; }
        /// <summary>
        /// 关注者openID
        /// </summary>
        public string openID { get; set; }

        [Timestamp]
        public int RecordVersion { get; set; }

        /// Matthew 20170419 - start
        /// <summary>
        /// 帐号
        /// </summary>
        public string accountName { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal amount { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// 级别-gradeId
        /// </summary>
        [Bounded(typeof(AccountGrade))]
        public int grade { get; set; }
        /// Matthew 20170419 - end

        //默认地址ID
        public int defaultAddressId { get; set; }
        /// Matthew 20170625 - end

    }

    public class AccountStates : States
    {
        /// <summary>
        /// 正常
        /// </summary>
        public const int normal = 1;

        /// <summary>
        /// 停用
        /// </summary>
        public const int blockup=2;
    }

    public class AccountGrade
    {
        /// <summary>
        /// 无
        /// </summary>
        public const int not = -1;
        /// <summary>
        /// 会员
        /// </summary>
        public const int Member = 0;
        /// <summary>
        /// 店主
        /// </summary>
        public const int Manager = 2;
        /// <summary>
        /// 店长
        /// </summary>

        public const int GoldMedalManager = 3;

        public static string GetName(int value)
        {
            switch (value)
            {
                case not:
                    return "无";
                case Member:
                    return "会员";
                case Manager:
                    return "店长";
                case GoldMedalManager:
                    return "店主";
                default:
                    return "无";
            }
        }
    }
}
