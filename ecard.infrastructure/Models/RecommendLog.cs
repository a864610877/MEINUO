using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 推荐记录
    /// </summary>
    public class RecommendLog
    {
        [Key]
        public int recommendLogId { get; set; }
        /// <summary>
        /// 推荐人UserId
        /// </summary>
        public int salerId { get; set; }
        /// <summary>
        /// 推荐人帐号
        /// </summary>
        public string salerName { get; set; }
        /// <summary>
        /// 被推荐人id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 被推荐人帐号
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }

    
}
