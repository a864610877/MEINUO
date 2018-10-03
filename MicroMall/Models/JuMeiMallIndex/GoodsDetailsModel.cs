using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class GoodsDetailsModel
    {
        public int commodityId { get; set; }

        /// <summary>
        /// 取第二张为details显示
        /// </summary>
        public string images { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal commodityPrice { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal commodityPrice1 { get; set; }
        public string commodityName { get; set; }

        /// <summary>
        /// 是否包邮
        /// </summary>
        public bool IsPinkage { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal commodityFreight { get; set; }

        /// <summary>
        /// 总价格=销售价格+运费
        /// </summary>
        public decimal totalPrice { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string commodityDetails { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int commodityInventory { get; set; }



    }
}