using Ecard.Models;
using Moonlit;
using System.Collections.Generic;
using Ecard.Infrastructure;

namespace Ecard.Services
{
    public interface IMembershipService
    { 
        User GetUserByName(string username);
        QueryObject<T> QueryUsers<T>(UserRequest request) where T : User;
        QueryObject<T> QueryUsersOfShops<T>(int? state, int? shopRole, params int[] shopIds) where T : User;
        void CreateUser(User user);
        User GetUserById(int id);
        void UpdateUser(User user);
        QueryObject<Role> QueryRoles(RoleRequest roleRequest);
        void DeleteUser(User user);
        void CreateRole(Role item);
        Role GetRoleById(int id);
        void UpdateRole(Role role);
        void DeleteRole(Role role);
        void DeleteUsersForRole(int roleId);
        void AssignRoles(User adminUser, int[] roleIds);
        void DeleteRolesForUser(int userId);
        void AssignUsers(Role role, int[] userIds);
        QueryObject<AdminUser> GetSales();
        QueryObject<AdminUser> GetSales(string Discriminator);
        QueryObject<T> GetUserByMobile<T>(string mobile, int exceptUserId);
        QueryObject<T> GetUserByPhoneNumber<T>(string phoneNumber, int exceptUserId);
        QueryObject<User> GetByIds(int[] ids);
        IEnumerable<int> GetUerIdsByRoleIds(int[] ids);
        

        User GetByMobile(string number);


        Account GetAccountById(int id);
        
        DataTables<Role> GetRoles(RoleRequest roleRequest);
        DataTables<AdminUser> NewQueryUser(UserRequest request);
    }

    public class RoleRequest
    {
        private string _name;
        private string _nameWith;
        private string _displayNameWith;
        public int? UserId { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? State { get; set; }

        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(_name) ? null : _name;
            }
            set
            {
                _name = value;
            }
        }

        public int[] RoldIds { get; set; }

        public string NameWith
        {
            get { return _nameWith.NullIfEmpty(); }
            set
            {
                _nameWith = value;
            }
        }

        public string DisplayNameWith
        {
            get { return _displayNameWith.NullIfEmpty(); }
            set
            {
                _displayNameWith = value;
            }
        }
    }

    public class UserRequest
    {
        private string _name;
        private string _nameWith;
        private string _displayNameWith;
        private string _emailWith;
        public int? RoleId { get; set; }
       // public string Discriminator { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Name
        {

            get
            {
                return string.IsNullOrEmpty(_name) ? null : _name;
            }
            set
            {
                _name = value;
            }
        }

        public int? State { get; set; }

        public int[] UserIds { get; set; }

        public string NameWith
        {
            get { return _nameWith.NullIfEmpty(); }
            set
            {
                _nameWith = value;
            }
        }

        public string DisplayNameWith
        {
            get
            {
                return _displayNameWith.NullIfEmpty();
            }
            set
            {
                _displayNameWith = value;
            }
        }

        public string EmailWith
        {
            get
            {
                return _emailWith.NullIfEmpty();
            }
            set
            {
                _emailWith = value;
            }
        }
         
    }
}