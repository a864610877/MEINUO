using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Orders
{
    public class OrderAttribute
    {
        //public Order item { get; set; }
        public List<CommodityAttribute> ListCommodity { get; set; }
    }

    public class CommodityAttribute
    {
        /// <summary>
        /// 购物车id
        /// </summary>
        public int shoppingCartId { get; set; }

        public int commodityId { get; set; }
        public string ImageUrl { get; set; }

        public string commodityName { get; set; }

        public decimal commodityPrice { get; set; }

        public decimal commodityFreight { get; set; }

        public string commodityRemark { get; set; }

        public string Attribute { get; set; }

        public int Num { get; set; }

    }
}