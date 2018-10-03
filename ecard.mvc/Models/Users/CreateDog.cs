using System;
using System.Text;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Models.Users
{
    public class CreateDog : ViewModelBase
    {
        public void Ready()
        {

        }

        [NoRender, Dependency]
        public IMembershipService MembershipService { get; set; }
        public string UserName { get; set; }
        public bool IsActived { get; set; }
        public void Read(int userId)
        {
            var adminUser = MembershipService.GetUserById(userId);
            if (adminUser != null)
            {
                UserName = adminUser.Name;
            }
        }

        public SimpleAjaxResult Create()
        {
            var serialNo = SerialNoHelper.Create();
            var user = MembershipService.GetUserByName(this.UserName);
            if (user == null)
                return new SimpleAjaxResult(Localize("nonFoundUser", UserName ?? ""));

            TransactionHelper.BeginTransaction();
            var guid = Guid.NewGuid().ToString("N").ToLower().Substring(0, 16);
            var token = BitConverter.ToString(Encoding.ASCII.GetBytes(guid)).Replace("-", "");
            user.LoginToken = token;
            user.LoginInToken = IsActived;
            MembershipService.UpdateUser(user);

            Logger.LogWithSerialNo(LogTypes.CreateDog, serialNo, user.UserId, user.DisplayName);
            return TransactionHelper.CommitAndReturn(new DataAjaxResult() { Data1 = token });
        }
    }
}