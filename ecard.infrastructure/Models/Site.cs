using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Moonlit;

namespace Ecard.Models
{
    /// <summary>
    /// This object represents the properties and methods of a Site.
    /// </summary>
    public class Site
    {
        public Site()
        {
           // State = 1;
        }

        [Key]
        public int SiteId { get; set; }

        public string CopyRight { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string FavIconUrl { get; set; }
        public decimal Version { get; set; }
        /// <summary>
        /// 默认password
        /// </summary>
        public string AuthType { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string imageUrl { get; set; }
        /// <summary>
        /// 推荐会员首次达最低消费赠送积分
        /// </summary>
        public int givePoint { get; set; }
        /// <summary>
        /// 推荐会员首次最低消费金额
        /// </summary>
        public decimal minAmount { get; set; }
        /// <summary>
        /// 推荐赠送积分
        /// </summary>
        public int recommendPoint { get; set; }
        /// <summary>
        /// 积分兑换比例
        /// </summary>
        public decimal PointRatio { get; set; }
        /// <summary>
        /// 推荐人获得会员消费返利比例
        /// </summary>
        public decimal RebateRatio { get; set; }
        /// <summary>
        /// 会员-推荐会员消费返积分比例
        /// </summary>
        public decimal OneRebate { get; set; }
        
        /// <summary>
        /// 经理-推荐会员消费返积分比例
        /// </summary>
        public decimal TwoRebate { get; set; }
        /// <summary>
        /// 成为经理需要直推的人数，且首次购物超过98
        /// </summary>
        public int TwoPeople { get; set; }
        /// <summary>
        /// 成为经理所需要的积分
        /// </summary>
        public int TwoPoint { get; set; }
        /// <summary>
        /// 金牌经理-推荐会员消费返积分比例
        /// </summary>
        public decimal ThreeRebate { get; set; }
        /// <summary>
        /// 成为金牌经理需要直推的人数，且首次购物超过98
        /// </summary>
        public int ThreePeople { get; set; }
        /// <summary>
        /// 成为金牌经理所需要的积分
        /// </summary>
        public int ThreePoint { get; set; }
        /// <summary>
        /// 后台url
        /// </summary>
        public string adminUrl { get; set; }
        /// <summary>
        /// 前端域名
        /// </summary>
        public string Url { get; set; }
 
        //public byte ServiceRetryCountDefault { get; set; }
        //public string RouteUrlPrefix { get; set; }
        //public bool CommentingDisabled { get; set; }
        //public string MixCode { get; set; }
        //public int State { get; set; }
        //public bool IsIKeySignIn { get; set; }
        //public string HowToDeals { get; set; }
       

    

      

        
        /// <summary>
        /// 短信帐号
        /// </summary>
        public string SmsAccount { get; set; }
        /// <summary>
        /// 短信密码
        /// </summary>
        public string SmsPwd { get; set; }
        /// <summary>
        /// 重发次数
        /// </summary>
        public int RetryCount { get; set; }

        //public IEnumerable<IdNamePair> GetBanks()
        //{
        //    return (Banks ?? "").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => new IdNamePair() { Name = x, Key = x.GetHashCode() });
        //}

        //public string GetBank(int v)
        //{
        //    return (from x in GetBanks()
        //            where x.Key == v
        //            select x.Name).FirstOrDefault();
        //}
    }
}