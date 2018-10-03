using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class ListCommodityByCategorys
    {
        public int PageIndex { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }

        public List<ListCommodityByCategory> List { get; set; }
    }

    public class ListCommodityByCategory
    {
        public int commodityId { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal commodityPrice { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string commodityName { get; set; }
        /// <summary>
        ///销售数量
        /// </summary>
        public int sellQuantity { get; set; }
        /// <summary>
        /// 商品说明
        /// </summary>
        public string commodityRemark { get; set; }
        /// <summary>
        /// 中图
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 大图
        /// </summary>
        public string MaxImg { get; set; }
    }
}