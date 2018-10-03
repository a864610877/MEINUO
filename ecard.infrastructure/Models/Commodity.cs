using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 商品信息表
    /// </summary>
    public class Commodity
    {
        [Key]
        public int commodityId { get; set; }

        /// <summary>
        /// 商品分类ID
        /// </summary>
        public int commodityCategoryId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        /// 商品编号 唯一
        /// </summary>
        public string commodityNo { get; set; }
        /// <summary>
        /// 排序 升序
        /// </summary>
        public int commodityRank { get; set; }
        /// <summary>
        /// 商品关键字 检索
        /// </summary>
        public string commdityKeyword { get; set; }
        /// <summary>
        /// 图片集合 多个 “,”隔开
        /// </summary>
        public string images { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal commodityPrice { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int commodityInventory { get; set; }
        /// <summary>
        /// 商品运费
        /// </summary>
        public decimal commodityFreight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string commodityRemark { get; set; }
        /// <summary>
        /// 商品介绍
        /// </summary>
        public string commodityDetails { get; set; }
        /// <summary>
        /// 商品状态，1上架 ，2下架 
        /// </summary>
        [Bounded(typeof(CommodityStates))]
        public int commodityState { get; set; }
        /// <summary>
        /// 规格ID,多个“，”隔开
        /// </summary>
        public string specificationId { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int sellQuantity { get; set; }

        ///Matthew - 20170419 start
        /// <summary>
        /// 原价
        /// </summary>
        public decimal commodityPrice1 { get; set; }

        /// <summary>
        /// 是否包邮
        /// </summary>
        public bool IsPinkage { get; set; }

        /// <summary>
        /// 实际销售数量
        /// </summary>
        public int sellQuantity1 { get; set; }
        ///Matthew - 20170419 end

        ///商品积分
        public decimal commodityJifen { get; set; }
    }

    public class CommodityStates
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
