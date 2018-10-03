using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.Models.Roles;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using Moonlit;
using ValidationError = Ecard.Validation.ValidationError;

namespace Ecard.Mvc.Models.AdminUsers
{
    public class CreateAdminUser : AdminUserModelBase, IValidator
    {
        public CreateAdminUser()
        {
            Roles = new MultiCheckList<ListRole>();
        }

        public CreateAdminUser(AdminUser adminUser)
            : base(adminUser)
        {
            Roles = new MultiCheckList<ListRole>();
            if (adminUser.Roles != null)
                Roles = new MultiCheckList<ListRole>(adminUser.Roles.ToList().Select(x => new ListRole(x)));
        }

        [StringLength(20)]
        [Required(ErrorMessage="请输入名称")]
        public string Name
        {
            get { return InnerObject.Name; }
            set { InnerObject.Name = value.TrimSafty(); }
        }

        [MultiCheckList,NoRender]
        public MultiCheckList<ListRole> Roles { get; set; }

        public IMessageProvider Save()
        {
            var serialNo = SerialNoHelper.Create();
            TransactionHelper.BeginTransaction();
            var roleIds = Roles.GetCheckedIds();
            InnerObject.BuildIn = false;
            InnerObject.State = UserStates.Normal;
            InnerObject.SetPassword(Password ?? "");
            if (string.IsNullOrEmpty(Password))
                InnerObject.SetPassword(Password);
            MembershipService.CreateUser(InnerObject);
            base.OnSaved(InnerObject);
            MembershipService.AssignRoles(InnerObject, roleIds);
            Logger.LogWithSerialNo(LogTypes.AdminUserCreate, serialNo,InnerObject.UserId, InnerObject.Name);
            AddMessage("success", InnerObject.Name);
            return TransactionHelper.CommitAndReturn(this);
        }

        public void Ready()
        {
            Roles.Merge(MembershipService.QueryRoles(new RoleRequest() { State = RoleStates.Normal }).Where(x => !x.BuildIn).Select(x => new ListRole(x)).ToList());
            OnReady();
        }

        public IEnumerable<ValidationError> Validate()
        {
            if (!string.IsNullOrEmpty(Name) && MembershipService.QueryUsers<User>(new UserRequest() { Name = Name }).Count() > 0)
                yield return new ValidationError("Name", string.Format(Localize("messages.duplicationUser"), InnerObject.Name));
        }
    }
}
