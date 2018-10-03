using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class OrangeKeyAndopenID
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 推荐码
        /// </summary>
        public string orangeKey { get; set; }
        /// <summary>
        /// 关注者openID
        /// </summary>
        public string openID { get; set; }
        /// <summary>
        /// 关于重试的消息排重，推荐使用FromUserName + CreateTime 排重
        /// </summary>
        public string messageId { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
