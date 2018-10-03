using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class Fz_Messages
    {
        [Key]
        public int messageId { get; set; }
        /// <summary>
        /// 用户openId
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int accountId { get; set; }
        /// <summary>
        /// 消息类型 1、订单状态  2提现申请通知  3、提现审核结果通知
        /// </summary>
        public int msgType { get; set; }
        /// <summary>
        /// 发送的消息
        /// </summary>
        public string msg { get; set; }
        public string keyword1 { get; set; }
        public string keyword2 { get; set; }
        public string keyword3 { get; set; }
        public string keyword4 { get; set; }
        public string keyword5 { get; set; }

        public int state { get; set; }
        public DateTime? sendTime { get; set; }
        public DateTime submitTime { get; set; }
    }


    public class MsgType
    {
        /// <summary>
        /// 订单状态消息
        /// </summary>
        public const int orderState = 1;
        /// <summary>
        /// 提现通知
        /// </summary>
        public const int withdrawNotice = 2;
        /// <summary>
        /// 提现审核
        /// </summary>
        public const int withdrawReview = 3;
    }

    public class MessagesState
    {
        /// <summary>
        /// 待发送
        /// </summary>
        public const int staySend = 1;
        /// <summary>
        /// 已完成
        /// </summary>
        public const int completed = 2;
    }
}
