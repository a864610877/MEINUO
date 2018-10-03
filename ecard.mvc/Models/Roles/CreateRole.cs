using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.Models.Permissions;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using Moonlit;
using ValidationError = Ecard.Validation.ValidationError;

namespace Ecard.Mvc.Models.Roles
{
    public class CreateRole : RoleModelBase, IValidator
    {
        [Required(ErrorMessage = "请输入名称")]
        [StringLength(20)]
        public  string Name
        {
            get { return InnerObject.Name; }
            set { InnerObject.Name = value.TrimSafty(); }
        }
        protected override string RoleName
        {
            get { return this.Name; }
        }

        public void Ready()
        {
            var permissions = PermissionService.QueryPermissions(this.UserType).Select(x => new ListPermission(x)).ToList();
            this.Permissions = new MultiCheckList<ListPermission>((this.InnerObject.Permissions ?? "").Split(',').Select(x => new ListPermission(new Permission() { Name = x })));
            Permissions.Merge(permissions);
        }
        public IMessageProvider Create()
        {
            var serialNo = SerialNoHelper.Create();
            TransactionHelper.BeginTransaction();
            var ids = Permissions.GetCheckedIds();
            var permissions = PermissionService.QueryPermissions(this.UserType).Where(x => ids.Contains(x.PermissionId)).Select(x => x.Name).ToArray();
            InnerObject.Permissions = string.Join(",", permissions);

            MembershipService.CreateRole(InnerObject);

            this.Logger.LogWithSerialNo(LogTypes.RoleCreate, serialNo, InnerObject.RoleId, InnerObject.Name);
            AddMessage("success", InnerObject.Name);
            return TransactionHelper.CommitAndReturn(this);
        }

        public IEnumerable<ValidationError> Validate()
        {
            if (string.IsNullOrEmpty(Name) && MembershipService.QueryRoles(new RoleRequest() { Name = Name }).Count() > 0)
                yield return new ValidationError("Name", string.Format(Localize("messages.duplicationUser"), InnerObject.Name));
        }
    }

}
