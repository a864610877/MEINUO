using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ShoppingCarts
{
    public class AddShoppingCarts
    {
        [Dependency]
        public ILog4netService logService { get; set; }
        [Dependency]
        public SecurityHelper _securityHelper { get; set; }
        [Dependency]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Dependency]
        public ICommodityService CommodityService { get; set; }

        public int num { get; set; }

        public int id { get; set; }

        public string attribute { get; set; }

        public ResultMessage  Save()
        {
            try
            {
                var user = _securityHelper.GetCurrentUser();
                if (user == null)
                    return new ResultMessage() { Code = -1, Msg = "您还没有登录呢！" };
                //if(user.CurrentUser.State!=UserStates.Normal)
                //    return new ResultMessage() { Code = -1, Msg = "您还没有登录呢！" };
                var commodity = CommodityService.GetById(id);
                if (commodity == null)
                    return new ResultMessage() { Code = -1, Msg = "商品不存在！" };
                if (commodity.commodityState == CommodityStates.soldOut)
                    return new ResultMessage() { Code = -1, Msg = "商品已下架！" };
                var Cart = ShoppingCartService.GetByUserIdAndCommodityId(user.CurrentUser.UserId,id);
                if (Cart != null)
                {
                    Cart.quantity = num;
                    Cart.specification = attribute;
                    Cart.submitTime = DateTime.Now;
                    ShoppingCartService.Update(Cart);
                }
                else
                {
                    var item = new ShoppingCart();
                    item.commodityId = commodity.commodityId;
                    item.quantity = num;
                    item.specification = attribute;
                    item.submitTime = DateTime.Now;
                    item.userId = user.CurrentUser.UserId;
                    ShoppingCartService.Insert(item);
                    
                }
                return new ResultMessage() { Code = 0 };
            }
            catch(Exception ex)
            {
                logService.Insert(ex);
                return new ResultMessage() { Code = -1, Msg = "系统错误！请联系管理员" };
            }
        }
    }
}