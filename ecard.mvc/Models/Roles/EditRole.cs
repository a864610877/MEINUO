using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.Models.AdminUsers;
using Ecard.Mvc.Models.Permissions;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;

namespace Ecard.Mvc.Models.Roles
{
    public class EditRole : RoleModelBase
    {
        [Hidden]
        public int RoleId
        {
            get { return InnerObject.RoleId; }
            set { InnerObject.RoleId = value; }
        }
        protected override string RoleName
        {
            get { return this.Name; }
        }
        public string Name
        {
            get { return InnerObject.Name.TrimSafty(); }
        }

        [MultiCheckList, NoRender]
        public MultiCheckList<ListAdminUser> Users { get; set; }

        public IMessageProvider Save()
        {
            var serialNo = SerialNoHelper.Create();
            TransactionHelper.BeginTransaction();
            var role = MembershipService.GetRoleById(InnerObject.RoleId);
            if (role == null) return this;

            role.DisplayName = DisplayName;
            if (Users != null && !role.BuildIn)
            {
                var roleIds = Users.GetCheckedIds();
                //role.Users = MembershipService.QueryUsers<User>(new UserRequest() { Ids = roleIds }).ToList();

                MembershipService.DeleteUsersForRole(role.RoleId);
                MembershipService.AssignUsers(role, roleIds);
            }
            var ids = this.Permissions.GetCheckedIds();
            var permissions = PermissionService.QueryPermissions(this.UserType).Where(x => ids.Contains(x.PermissionId)).Select(x => x.Name).ToArray();
            role.Permissions = string.Join(",", permissions);
            MembershipService.UpdateRole(role);
            this.Logger.LogWithSerialNo(LogTypes.RoleEdit, serialNo, role.RoleId, InnerObject.Name);
            AddMessage("success", InnerObject.Name);
            return TransactionHelper.CommitAndReturn(this);
        }
        public void Ready()
        {
            Users = new MultiCheckList<ListAdminUser>();
            if (InnerObject.Users != null)
                Users = new MultiCheckList<ListAdminUser>(InnerObject.Users.OfType<AdminUser>().ToList().Select(x => new ListAdminUser(x)));
            Users.Merge();

            var permissions = PermissionService.QueryPermissions(this.UserType).Select(x => new ListPermission(x)).ToList();
            this.Permissions = new MultiCheckList<ListPermission>((this.InnerObject.Permissions ?? "").Split(',').Select(x => new ListPermission(new Permission() { Name = x })));
            Permissions.Merge(permissions);
        }

        public void Read(int id)
        {
            SetInnerObject(MembershipService.GetRoleById(id));
        }

    }
}