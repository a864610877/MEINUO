using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Accounts
{
    public class EditAccount : EcardModelListRequest<Account>
    {

        [Dependency]
        [NoRender]
        public IAccountService accountService { get; set; }
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        [Dependency]
        [NoRender]
        public IMembershipService membershipService { get; set; }
        [Dependency]
        [NoRender]
        public IProvinceService ProvinceService { get; set; }
        [Dependency]
        [NoRender]
        public ICityService CityService { get; set; }

        [NoRender]
        /// <summary>
        /// 会员ID
        /// </summary>
        public int accountId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string passWord { get; set; }
        public string repassWord { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string eamil { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime registerDate { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? birthDay { get; set; }
        /// <summary>
        /// 赠送积分
        /// </summary>
        public decimal presentExp { get; set; }
        /// <summary>
        /// 未激活积分
        /// </summary>
        public decimal notActivatePoint { get; set; }
        /// <summary>
        /// 激活积分
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 消费积分汇总
        /// </summary>
        public decimal payPoint { get; set; }
        /// <summary>
        /// 提现积分汇总
        /// </summary>
        public decimal withdrawPoint { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        public string referrer { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        public int provinceId { get; set; }
        public int cityId { get; set; }

        public List<IdNamePair> ListCity { get; set; }
        public List<IdNamePair> ListProvince { get; set; }
        public void Ready(int id)
        {
            var account = accountService.GetById(id);
            if (account != null)
            {
               var user = membershipService.GetUserById(account.userId);
               var saler = membershipService.GetUserById(account.salerId);
               if (user != null)
               {
                   this.accountId = account.accountId;
                   this.accountName = user.DisplayName;
                   this.birthDay = user.BirthDate;
                   this.activatePoint = account.activatePoint;
                   this.eamil = user.Email;
                  // this.notActivatePoint = account.notActivatePoint;
                   //this.payPoint = account.payPoint;
                   this.phone = user.PhoneNumber;
                   this.presentExp = account.presentExp;
                   this.referrer = saler == null ? "" : saler.DisplayName;
                   this.registerDate = account.submitTime;
                   this.state = account.state;
                  // this.withdrawPoint = account.withdrawPoint;
                   this.provinceId = user.provinceId;
                   this.cityId = user.cityId;
                   this.address = user.Address;
               }
               var query = ProvinceService.Query();
               if (query != null)
                   ListProvince = query.Select(x => new IdNamePair() { Key = x.ProvinceId, Name = x.Name }).ToList();

               var query1 = CityService.Query(provinceId);
               if (query1 != null)
                   ListCity = query1.Select(x => new IdNamePair() { Key = x.CityId, Name = x.Name }).ToList();
            }
        }

        public ResultMsg Save()
        {
            ResultMsg result = new ResultMsg();

            if (passWord != null && repassWord != null)
            {
                if (!passWord.Equals(repassWord))
                {
                    result.Code = 3;
                    result.CodeText = "两次输入密码不一致，请重新输入";
                    return result;
                }
            }

            var account = accountService.GetById(this.accountId);
            var user = membershipService.GetUserById(account.userId);
            if (account != null && user != null)
            {
                transaction.BeginTransaction();
                user.DisplayName = this.accountName;
                user.PhoneNumber = this.phone;
                user.Email = this.eamil;
                user.provinceId = provinceId;
                user.cityId = cityId;
                user.Address = address;
                user.BirthDate = birthDay;
                if(!string.IsNullOrWhiteSpace(this.passWord))
                     user.SetPassword(this.passWord);
                user.State = this.state;
                membershipService.UpdateUser(user);
                account.state = this.state;

                accountService.Update(account);
                transaction.Commit();
                result.Code = 1;
                result.CodeText = "修改成功!";
                return result;
            }
            else
            {
                result.Code = 2;
                result.CodeText = "修改失败!";
                return result;
            }

        }
    }
}
