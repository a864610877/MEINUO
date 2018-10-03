using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.Models.Roles;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models.AdminUsers
{
    public class EditAdminUser : AdminUserModelBase
    {
        public EditAdminUser()
        {
            Roles = new MultiCheckList<ListRole>();
        }
        [Hidden]
        public int UserId
        {
            get { return InnerObject.UserId; }
            set { InnerObject.UserId = value; }
        }
        public EditAdminUser(AdminUser adminUser)
            : base(adminUser)
        {
            Roles = new MultiCheckList<ListRole>();
            if (adminUser.Roles != null)
                Roles = new MultiCheckList<ListRole>(adminUser.Roles.Select(x => new ListRole(x)).ToList());
        }

        public string Name
        {
            get { return InnerObject.Name.TrimSafty(); }
        }

        [Display(Order = 1000)]
        [MultiCheckList,NoRender]
        public MultiCheckList<ListRole> Roles { get; set; }

        public void Save()
        {
            var serialNo = SerialNoHelper.Create();
            var oldUser = MembershipService.GetUserById(InnerObject.UserId);
            var adminUser = oldUser as AdminUser;
            if (adminUser == null)
                return;
            TransactionHelper.BeginTransaction();

            adminUser.Email = Email;
            adminUser.DisplayName = DisplayName;
            if (!string.IsNullOrEmpty(Password))
            {
                adminUser.SetPassword(Password);
            }
            adminUser.BirthDate = BirthDate;
            adminUser.Mobile = Mobile;
            //adminUser.IsSale = IsSale;
            var roleIds = Roles.GetCheckedIds();
            MembershipService.UpdateUser(adminUser);
            OnSaved(adminUser);
            MembershipService.DeleteRolesForUser(adminUser.UserId);
            MembershipService.AssignRoles(adminUser, roleIds);
            AddMessage("success", oldUser.Name);
            Logger.LogWithSerialNo(LogTypes.AdminUserEdit, serialNo, adminUser.UserId, oldUser.Name);
            TransactionHelper.Commit();
        }

        public void Ready()
        {
            Roles.Merge(MembershipService.QueryRoles(new RoleRequest() { State = UserStates.Normal }).Where(x => !x.BuildIn).Select(x => new ListRole(x)).ToList());
            base.OnReady();
        }

        public void Read(int id)
        {
            var user = MembershipService.GetUserById(id);
            var adminUser = user as AdminUser;
            if (adminUser != null && adminUser.Roles != null)
                Roles = new MultiCheckList<ListRole>(user.Roles.Select(x => new ListRole(x)).ToList());

            base.SetInnerObject(adminUser);
        }
    }
}