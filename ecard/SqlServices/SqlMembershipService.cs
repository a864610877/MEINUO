using System;
using System.Collections.Generic;
using System.Linq;
using Ecard.Models;
using Moonlit.Data;
using Ecard.Infrastructure;
using System.Data.SqlClient;
namespace Ecard.Services
{
    public class SqlMembershipService : IMembershipService
    {
        private readonly DatabaseInstance _databaseInstance;
        private const string UserTableName = "Users";
        private const string RoleTableName = "Roles";
        private const string UserRoleTableName = "UserRoles";
        private const string AccountTableName = "fz_Accounts";
        public SqlMembershipService(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }

        public User GetUserByName(string username)
        {
            var user = QueryUsers<User>(new UserRequest { Name = username ?? "fkslfj38fjejfskldjfjefjkl" }).AsEnumerable().FirstOrDefault();
            FillRole(user);
            return user;
        }

        public QueryObject<T> QueryUsers<T>(UserRequest request) where T : User
        {
            var sql =
                @"select t.* from users t where 
(@RoleId is null or t.UserId in (Select ur.User_UserId from UserRoles ur where ur.Role_RoleId = @RoleId))
and (@State is null or t.State = @State)
and (@Name is null or t.Name = @Name)
and (@NameWith is null or t.Name like '%' + @NameWith + '%')
and (@DisplayNameWith is null or t.DisplayName like '%' + @DisplayNameWith + '%')
and (@EmailWith is null or t.Email like '%' + @EmailWith + '%')
and (@UserIds is null or t.UserId in (@UserIds))
and (@Discriminator is null or t.Discriminator = @Discriminator) ";
            return new QueryObject<T>(_databaseInstance, sql, request);
        }

        public QueryObject<T> QueryUsersOfShops<T>(int? state, int? shopRole, params int[] shopIds) where T : User
        {
            return new QueryObject<T>(_databaseInstance, "userOfShop.query", new { state = state, shopRole = shopRole, shopIds = shopIds });
        }

        public void CreateUser(User user)
        {
            user.UserId = _databaseInstance.Insert(user, UserTableName);
        }

        public void CreateRole(Role item)
        {
            item.RoleId = _databaseInstance.Insert(item, RoleTableName);
        }

        public User GetUserById(int id)
        {
            var user = _databaseInstance.GetById<User>(UserTableName, id);
            FillRole(user);
            return user;
        }

        private void FillRole(User user)
        {
            if (user != null)
            {
                user.Roles = GetRolesOfUser(user.UserId);
            }
        }

        private List<Role> GetRolesOfUser(int userId)
        {
            var sql =
                @"select t.* from roles t where 
                            t.RoleId in (Select ur.Role_RoleId from UserRoles ur where ur.User_UserId = @UserId)";
            return new QueryObject<Role>(_databaseInstance, sql, new { UserId = userId }).ToList();
        }

        public Role GetRoleById(int id)
        {
            var role = _databaseInstance.GetById<Role>(RoleTableName, id);

            if (role != null)
            {
                role.Users = QueryUsers<AdminUser>(new UserRequest { RoleId = id }).ToList().Cast<User>().ToList();
            }
            return role;
        }

        public void UpdateUser(User user)
        {
            _databaseInstance.Update(user, UserTableName);
        }

        public void UpdateRole(Role role)
        {
            _databaseInstance.Update(role, RoleTableName);
        }


        public QueryObject<Role> QueryRoles(RoleRequest roleRequest)
        {
            var sql =
                @"select t.* from roles t where 
(@UserId is null or t.RoleId in (Select ur.Role_RoleId from UserRoles ur where ur.User_UserId = @UserId))
and (@Name is null or @Name = Name)
and (@NameWith is null or Name like @NameWith)
and (@DisplayNameWith is null or DisplayName like @DisplayNameWith)
and (@State is null or State = @State)
";
            return new QueryObject<Role>(_databaseInstance, sql, roleRequest);
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        public DataTables<Role> GetRoles(RoleRequest roleRequest)
        {
            SqlParameter[] param = { 
                                         new SqlParameter("@Name",roleRequest.Name),
                                        new SqlParameter("@pageIndex",roleRequest.PageIndex),
                                        new SqlParameter("@pageSize",roleRequest.PageSize),
                                        new SqlParameter("@DisplayNameWith",roleRequest.DisplayNameWith),
                                        new SqlParameter("@NameWith",roleRequest.NameWith),
                                        new SqlParameter("@UserId",roleRequest.UserId),
                                        new SqlParameter("@State",roleRequest.State)
                                   };
            StoreProcedure sp = new StoreProcedure("P_getRoles", param);
            return    _databaseInstance.GetTables<Role>(sp);
        }
        public DataTables<AdminUser> NewQueryUser(UserRequest request)
        {
            SqlParameter[] param = { 
                                   new SqlParameter("@RoleId",request.RoleId),
                                   new SqlParameter("@pageIndex",request.PageIndex),
                                   new SqlParameter("@pageSize",request.PageSize),
                                   new SqlParameter("@State",request.State),
                                   new SqlParameter("@Name",request.Name),
                                   new SqlParameter("@NameWith",request.NameWith),
                                   new SqlParameter("@DisplayNameWith",request.DisplayNameWith),
                                   new SqlParameter("@EmailWith",request.EmailWith)
                                   };
            StoreProcedure sp = new StoreProcedure("P_getUsers",param);
            return _databaseInstance.GetTables<AdminUser>(sp);
        }
        public void DeleteUser(User user)
        {
            _databaseInstance.Delete(user, UserTableName);
        }

        public void DeleteRole(Role role)
        {
            _databaseInstance.Delete(role, RoleTableName);
        }

        public void DeleteUsersForRole(int roleId)
        {
            var sql = @"delete from UserRoles where Role_RoleId = @roleId";
            _databaseInstance.ExecuteNonQuery(sql, new { roleId = roleId });
        }

        public void DeleteRolesForUser(int userId)
        {
            var sql = @"delete from UserRoles where User_UserId = @userId 
                and Role_RoleId in (select roleId from roles where buildin = 0)";
            _databaseInstance.ExecuteNonQuery(sql, new { userId = userId });
        }

        public void AssignUsers(Role role, int[] userIds)
        {
            foreach (var userId in userIds)
            {
                _databaseInstance.Insert(new UserRole { Role_RoleId = role.RoleId, User_UserId = userId }, UserRoleTableName);
            }
        }

        public QueryObject<AdminUser> GetSales()
        {
            return new QueryObject<AdminUser>(_databaseInstance, "user.sales", null);
        }

        public QueryObject<T> GetUserByMobile<T>(string mobile, int exceptUserId)
        {
            return new QueryObject<T>(_databaseInstance, "user.GetUserByMobile", new { Mobile = mobile, exceptUserId = exceptUserId });
        }
        public QueryObject<T> GetUserByPhoneNumber<T>(string phoneNumber, int exceptUserId)
        {
            return new QueryObject<T>(_databaseInstance, "user.GetUserByPhoneNumber", new { phoneNumber = phoneNumber, exceptUserId = exceptUserId });
        }

        public QueryObject<User> GetByIds(int[] ids)
        {
            return new QueryObject<User>(_databaseInstance, "select * from users where userid in (@ids)", new { ids = ids });
        }

        public void AssignRoles(User adminUser, int[] roleIds)
        {
            foreach (var roleId in roleIds)
            {
                _databaseInstance.Insert(new UserRole { Role_RoleId = roleId, User_UserId = adminUser.UserId }, UserRoleTableName);
            }
        }

        public class UserRole
        {
            public int Role_RoleId { get; set; }
            public int User_UserId { get; set; }
        }


        public QueryObject<AdminUser> GetSales(string Discriminator)
        {
            return new QueryObject<AdminUser>(_databaseInstance, "user.sales", Discriminator);
        }


        public IEnumerable<int> GetUerIdsByRoleIds(int[] ids)
        {
            var users = new QueryObject<UserRole>(_databaseInstance, "select * from UserRoles where Role_RoleId in (@ids)", new { ids = ids });
            var result = users.Select(x => x.User_UserId);
            return result;
        }


        public User GetByMobile(string number)
        {
            string sql = "select top 1 * from users where  Mobile=@number and Discriminator='AccountUser'";
            return new QueryObject<User>(_databaseInstance, sql, new { number = number }).FirstOrDefault();
        }


        public Account GetAccountById(int id)
        {
            var Account = _databaseInstance.GetById<Account>(AccountTableName, id);
            //FillRole(Account);
            return Account;
        }
    }
}