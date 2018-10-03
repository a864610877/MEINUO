using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class SecondKillCommoditys
    {
        [Key]
        public int id { get; set;}

        public int commodityId { get; set; }

        public string commodityNo { get; set; }
        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 秒杀数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int surplusNum { get; set; }
        /// <summary>
        /// 已购数量
        /// </summary>
        public int payNum { get; set; }
        public DateTime createTime { get; set; }
    }

    public class SecondKillCommoditysss : SecondKillCommoditys
    {
        public string commodityNo { get; set; }

        public string commodityName { get; set; }

        public string images { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal commodityPrice { get; set; }
    }
}
