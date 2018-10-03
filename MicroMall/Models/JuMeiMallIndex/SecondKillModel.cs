using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class SecondKillModel
    {
        /// <summary>
        /// 秒杀开始时间
        /// </summary>
        public DateTime? startTime { get; set; }

        /// <summary>
        /// 秒杀结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        /// 秒杀状态 0未开启 1开启
        /// </summary>
        public int state { get; set; }




        public List<SecondKillCommodityss> ListCommodity { get; set; }
    }

    public class SecondKillCommodityss
    {
        public int id { get; set; }
        public int commodityId { get; set; }
        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 原价格
        /// </summary>
        public decimal yPrice { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int surplusNum { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string img { get; set; }
    }
}