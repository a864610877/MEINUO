using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models.Users
{
    public class RecoveryUser : ViewModelBase
    {
        [Required(ErrorMessage = "请输入名称")]
        public string Name { get; set; }
        [NoRender, Dependency]
        public ITemporaryTokenKeyService TemporaryTokenKeyService { get; set; }
        [NoRender, Dependency]
        public IMembershipService MembershipService { get; set; }
        [NoRender, Dependency]
        public EmailService EmailService { get; set; }

        public SimpleAjaxResult Save()
        {
            try
            {
                Name = Name ?? "";
                var serialNo = SerialNoHelper.Create();
                var user = MembershipService.GetUserByName(Name);
                if (user == null)
                {
                    Logger.Error(LogTypes.RecoveryPassword, "用户名 " + Name + " 不存在或电子邮件错！");
                    return new SimpleAjaxResult("用户名 " + Name + " 不存在或电子邮件错！");
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    Logger.Error(LogTypes.RecoveryPassword, "用户名 " + Name + " 不存在或电子邮件错！");
                    return new SimpleAjaxResult("用户名 " + Name + " 不存在或电子邮件错！");
                }
                var items = TemporaryTokenKeyService.QueryByUser(TokenKeyTypes.RecoveryPassword, user.Name).ToList();
                foreach (var item in items)
                {
                    TemporaryTokenKeyService.Delete(item);
                }
                Logger.LogWithSerialNo(LogTypes.RecoveryPassword, serialNo, user.UserId, user.Name);

                TemporaryTokenKey key = new TemporaryTokenKey
                                            {
                                                ExpiredDate = DateTime.Now.Date.AddDays(7),
                                                TokenKeyType = TokenKeyTypes.RecoveryPassword,
                                                UserName = user.Name,
                                                Token = Guid.NewGuid().ToString()
                                            };

                TemporaryTokenKeyService.Create(key);
                var context = new Dictionary<string, string>
                              {
                                  {"userName", user.Name},
                                  {"userDisplayName", user.DisplayName},
                                  {"token", key.Token},
                              };
                EmailService.SendMail("recoveryPassword", user.Email, "重置密码申请", context);
                return new SimpleAjaxResult();
            }
            catch (Exception ex)
            {
                Logger.Error(LogTypes.RecoveryPassword, ex);
                return new SimpleAjaxResult("重置密码失败: " + ex.Message);
            }
        }
    }
}
