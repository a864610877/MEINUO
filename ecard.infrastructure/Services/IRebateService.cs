using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IRebateService
    {
        int Insert(fz_Rebate item);
        decimal GetRebateAmount(int accountId);
        /// <summary>
        /// 返佣
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Rebate(int orderId);
        /// <summary>
        /// 极差返利
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Rebate2(int orderId);
        /// <summary>
        /// 商城购物返利
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Rebate3(int orderId);
        /// <summary>
        /// 推荐注册返利
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool Rebate4(int orderId);
    }
}
