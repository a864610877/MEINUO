using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
//using Ecard.Mvc.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Insert(Account item);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Update(Account item);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int Delete(Account item);
        /// <summary>
        /// 根据id，获取实体
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Account GetById(int accountId);
        /// <summary>
        /// 更加UserId获取实体
        /// </summary>
        /// <param name="owenrId"></param>
        /// <returns></returns>
        Account GetByUserId(int userId);
        /// <summary>
        /// 根据openID获取实体
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        Account GetByopenID(string openID);
        /// <summary>
        /// 推荐码
        /// </summary>
        /// <param name="orangeKey"></param>
        /// <returns></returns>
        Account GetByorangeKey(string orangeKey);

        DataTables<AccountModel> Query(AccountRequest request);
        /// <summary>
        /// 销售额查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">0全部 1一级团队 2二级团队 3三级团队</param>
        /// <returns></returns>
        decimal SaleAmount(int id, int type);
        /// <summary>
        /// 销售团队
        /// </summary>
        /// <param name="request">1一级团队 2二级团队 3三级团队</param>
        /// <returns></returns>
        DataTables<AccountSale> GetSaleTeam(AccountSaleRequest request);
        /// <summary>
        /// 根据手机号，获取实体
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Account GetByMobile(string Mobile);
        /// <summary>
        /// 获取推荐会员级别以上的数量
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        int GetSalerCount(int accountId);
    }

    public class AccountSale
    {
        public int accountId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string grade { get; set; }
    }

    public class AccountSaleRequest : PageRequest
    {
        public int Id { get; set; }
        public int type { get; set; }
    }
}
