using Ecard.Requests;
using Moonlit;
using System;

namespace Ecard.Services
{
    public class CommodityRequest : PageRequest
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string commodityNo { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string commdityKeyword { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public int? commodityState { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startSubmitTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endSubmitTime { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public int? commodityCategoryId { get; set; }
    }
}