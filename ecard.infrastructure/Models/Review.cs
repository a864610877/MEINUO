using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int CommodityId { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态 1待审核 2显示 3不显示
        /// </summary>
        [Bounded(typeof(ReviewStates))]
        public int State { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime SubmitTime { get; set; }
    }

    public class ReviewExpress: Review
    {
        /// <summary>
        /// 会员名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 会员头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CateName { get; set; }
    }

    public class Reviewss : Review
    {
        public string UserName { get; set; }

        public string commodityName { get; set; }

        public string commodityNo { get; set; }
    }

    public class ReviewStates
    {
        public const int All = 0;
        /// <summary>
        /// 待审核
        /// </summary>
        public const int StayCheck = 1;
        /// <summary>
        /// 显示
        /// </summary>
        public const int Show = 2;
        /// <summary>
        /// 不显示
        /// </summary>
        public const int NotShow = 3;
    }
}
