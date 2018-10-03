using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class Ad
    {
        [Key]
        public int adId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string link { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Bounded(typeof(AdStates))]
        public int State { get; set; }
        /// <summary>
        /// 排序 升序
        /// </summary>
        public int rank { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImageUrl { get; set; }
    }

    public class AdStates
    {
        /// <summary>
        /// 全部
        /// </summary>
        public const int all = 0;
        /// <summary>
        /// 上架
        /// </summary>
        public const int putaway = 1;
        /// <summary>
        /// 下架
        /// </summary>
        public const int soldOut = 2;
    }
}
