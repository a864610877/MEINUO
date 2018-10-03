using System;
using System.ComponentModel.DataAnnotations;
using Ecard.Models;
using Ecard.Mvc.Models.Permissions;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;

namespace Ecard.Mvc.Models.Roles
{
    public class RoleModelBase : ViewModelBase
    {
        private Role _innerObject;
        [Dependency]
        [NoRender]
        public UserType UserType
        {
            get
            {
                if (string.Equals(this.RoleName, "shop", StringComparison.OrdinalIgnoreCase)) return UserType.ShopUser;
                if (string.Equals(this.RoleName, "distributor", StringComparison.OrdinalIgnoreCase)) return UserType.DistributorUser;
                if (string.Equals(this.RoleName, "account", StringComparison.OrdinalIgnoreCase)) return UserType.AccountUser;
                return UserType.AdminUser;
            }
        }
        public RoleModelBase()
        {
            _innerObject = new Role();
            _innerObject.State = States.Normal;
        }

        [Dependency]
        [NoRender]
        public IPermissionService PermissionService { get; set; }

        [Dependency]
        [NoRender]
        public IMembershipService MembershipService { get; set; }


        [NoRender]
        public Role InnerObject
        {
            get { return _innerObject; }
        }

        [MultiCheckList, NoRender]
        public MultiCheckList<ListPermission> Permissions { get; set; }

        [Required(ErrorMessage = "«Î ‰»Îœ‘ æ√˚≥∆")]
        [StringLength(40)]
        public string DisplayName
        {
            get { return _innerObject.DisplayName; }
            set { _innerObject.DisplayName = value.TrimSafty(); }
        }

        protected virtual string RoleName
        {
            get { return null; }
        }

        protected void SetInnerObject(Role innerObject)
        {
            _innerObject = innerObject;
        }
    }
}