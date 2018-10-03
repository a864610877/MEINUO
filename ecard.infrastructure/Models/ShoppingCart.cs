using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCart
    {
        [Key]
        public int shoppingCartId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        public int commodityId { get; set; }
        /// <summary>
        /// 规格描述
        /// </summary>
        public string specification { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; } 
    }

    public class UserShoppingCarts : ShoppingCart
    {
        public string images { get; set; }

        public string commodityName { get; set; }

        public string commodityRemark { get; set; }

        public decimal commodityPrice { get; set; }
    }
}
