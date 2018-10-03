using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 文章列表 -- Matthew 20170419
    /// </summary>
    public class fz_Articles
    {
        /// <summary>
        /// 主键 自增
        /// </summary>
        [Key]
        public int articleId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 略缩图
        /// </summary>
        public string imageUrl { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string describe { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime submitTime { get; set; }

    }
}
