using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ShoppingCarts
{
    public class MyCart : LayoutModel
    {
        public List<ShoppingCart> List { get; set; }
       
        [Dependency]
        public IShoppingCartService ShoppingCartService { get; set; }

        public void Query()
        {
            var user = _securityHelper.GetCurrentUser();
            if(user!=null)
            {

            }
        }
    }
}