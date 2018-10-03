﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class Articles
    {
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
        /// 时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
