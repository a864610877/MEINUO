using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Ecard.Models
{ 
    public class Role : IKeyObject, IRole, IDescriptionEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool BuildIn { get; set; }
        [StringLength(4000)]
        public string Permissions { get; set; }
        /// <summary>
        /// 超级管理员
        /// </summary>
        public bool IsSuper { get; set; }
        public void SetPermissions(string[] permissions)
        {
            this.Permissions = string.Join(",", permissions);
        }
        public bool HasPermissions(string permission)
        {
            if (IsSuper) return true;
            if(string.IsNullOrEmpty(Permissions)) return false;
            return this.Permissions.Split(',').Any(x => string.Equals(permission, x, StringComparison.InvariantCultureIgnoreCase));
        }

        public int Id
        {
            get { return RoleId; }
        }

        [Bounded(typeof(RoleStates))]
        public int State { get; set; }

        public ICollection<User> Users { get; set; }
    }

    public class RoleNames
    {
        public const string ShopOwner = "shopowner";
        public const string DistributorOwner = "distributorowner";
        public const string Account = "account";
    }
}