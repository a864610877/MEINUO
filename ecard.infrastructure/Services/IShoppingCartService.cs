using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IShoppingCartService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(ShoppingCart item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(ShoppingCart item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(ShoppingCart item);
        /// <summary>
        /// 根据id 获取实体
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <returns></returns>
        ShoppingCart GetById(int shoppingCartId);
        /// <summary>
        /// 根据用户id 获取，用户购物车列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTables<UserShoppingCarts> GetByUserId(ShoppingCartRequest request);

        ShoppingCart GetByUserIdAndCommodityId(int userId, int commodityId);


        DataTables<ShoppingCart> GetByAccountId(int accountId);
     
    }
}
