using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IUserAddressService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(UserAddress item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(UserAddress item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(UserAddress item);
        /// <summary>
        /// 根据id 获取实体
        /// </summary>
        /// <param name="userAddressId"></param>
        /// <returns></returns>
        UserAddress GetById(int userAddressId);

        DataTables<UserAddress> GetByUserId(UserAddressRequest request);

        DataTables<UserAddress> GetByAccountId(int accountId);
    }
}
