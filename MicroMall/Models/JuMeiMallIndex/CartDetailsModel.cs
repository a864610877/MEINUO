using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    /// <summary>
    /// 购物车详情
    /// </summary>
    public class CartDetailsModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 销售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriPrice { get; set; }
        /// <summary>
        /// 库存剩余数量
        /// </summary>
        public int quantity { get; set; }

        public int CommodityStock { get; set; }

        public string Image { get; set; }

        public int Id { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public int commodityId { get; set; }
        public decimal Freight { get; set; }
        public string specification { get; set; }


        public CartDetailsModel()
        {
            

        }
    }

    public class ListCartDetail
    {
        public ListCartDetail()
        {
            CartDetailsList = new List<CartDetailsModel>();
        }

        public List<CartDetailsModel> CartDetailsList { get; set; }

        /// <summary>
        /// 合计总金额
        /// </summary>
        public decimal TotalAmt { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

    }
    
}