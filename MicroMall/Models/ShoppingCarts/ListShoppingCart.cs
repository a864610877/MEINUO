using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ShoppingCarts
{
    public class ListShoppingCart
    {
        private readonly UserShoppingCarts _innerObject;
        public UserShoppingCarts InnerObject
        {
            get { return _innerObject; }
        }

        public ListShoppingCart()
        {
            _innerObject = new UserShoppingCarts();
        }
        public ListShoppingCart(UserShoppingCarts innerObject)
        {
            _innerObject = innerObject;
        }

        public int shoppingCartId { get { return InnerObject.shoppingCartId; } }

        public int commodityId { get { return InnerObject.commodityId; } }

        public string specification { get { return InnerObject.specification; } }

        public int quantity { get { return InnerObject.quantity; } }

        public string ImageUrl { get; set; }

        public string commodityName { get { return InnerObject.commodityName; } }

        public string commodityRemark { get { return InnerObject.commodityRemark; } }

        public decimal commodityPrice { get { return InnerObject.commodityPrice; } }

        public string images { get { return InnerObject.images; } }
    }
}