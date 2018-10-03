using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.ViewModels;
using Ecard.Services;

namespace Ecard.Mvc
{
    public class SecurityHelper
    {
        private readonly IMembershipService _membershipService;
        //private readonly IShopService _shopService;
        private readonly IAccountService _accountService;
        //private readonly IDistributorService _distributorService;

        public SecurityHelper(IMembershipService membershipService, IAccountService accountService)
        {
            _membershipService = membershipService;
            //_shopService = shopService;
            _accountService = accountService;
            //_distributorService = distributorService;
        }

        private const string KeyCurrentUser = "__currentUser_";
        private UserModel CreateUserModel(User user)
        {
            AdminUser adminUser = user as AdminUser;
            if (adminUser != null) return new AdminUserModel(adminUser);

            //ShopUser shopUser = user as ShopUser;
            //if (shopUser != null) return new ShopUserModel(shopUser, _shopService.GetById(shopUser.ShopId));

            AccountUser accountUser = user as AccountUser;
            if (accountUser != null) return new AccountUserModel(accountUser, _accountService.GetByUserId(accountUser.UserId));

            //DistributorUser distributorUser = user as DistributorUser;
            //if (distributorUser != null) return new DistributorUserModel(distributorUser, _distributorService.GetByUserId(distributorUser.UserId));
            throw new NotSupportedException(user.GetType().FullName);
        }
        public void Clear(string username)
        {
            var context = HttpContext.Current;
            var key = KeyCurrentUser + username;
            context.Cache.Remove(key);
        }

        public bool HasPermission(string permission)
        {
            var user = GetCurrentUser();
            if (user == null) return false;
            var roles = user.CurrentUser.Roles.ToList();
            if (roles[0].IsSuper)
                return true;
            else
                return roles[0].Permissions.Contains(permission);
        }

        public UserModel GetCurrentUser()
        {
            var context = HttpContext.Current;
            if (!context.Request.IsAuthenticated)
                return null;
            var key = KeyCurrentUser + context.User.Identity.Name;
            lock (String.Intern(key))
            {
                var userModel = (UserModel)context.Cache[key];
                if (userModel == null)
                {
                    var user = _membershipService.GetUserByName(context.User.Identity.Name);
                    userModel = CreateUserModel(user);
                    context.Cache.Add(key, userModel, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero, CacheItemPriority.High, null);
                }
                return userModel;
            }
        }
        public bool HasPermission(params string[] permissions)
        {
            var user = GetCurrentUser();
            if (user == null) return false;

            var roles = user.CurrentUser.Roles.ToList();
            if (roles[0].IsSuper == true)
                return true;
            return roles.AsQueryable().Any(x => permissions.Contains(x.Name));
        }






        //public bool CheckShop(IShopId item, bool checkType)
        //{
             
        //    //var user = this.GetCurrentUser().CurrentUser;
        //    //ShopUser u = user as ShopUser;
        //    //if (u == null)
        //    //{
        //    //    return !checkType;
        //    //}
        //    //return u.ShopId == item.ShopId;
        //}

        //public bool CheckAccountUser(IAccountId item, bool checkType)
        //{
        //    return false;
        //    //var user = this.GetCurrentUser().CurrentUser;
        //    //var u = user as AccountUser;
        //    //if (u == null)
        //    //{
        //    //    return !checkType;
        //    //}
        //    //var accounts = _accountService.QueryByOwnerId(u);
        //    //return accounts.Any(x => x.AccountId == item.AccountId);
        //}

    }
    public static class ExpressionExtensions
    {
        public static RouteValueDictionary ToRouteValueDictionary(this LambdaExpression expression, object routeValues)
        {
            MethodCallExpression callExpression = (MethodCallExpression)expression.Body;
            var type = callExpression.Method.DeclaringType;
            var controller = type.Name.Substring(0, type.Name.Length - 10);
            var action = callExpression.Method.Name;
            return routeValues.ToRouteValueDictionary(controller, action);
        }
        public static RouteValueDictionary ToRouteValueDictionary(this object routeValues)
        {
            var dictionary = routeValues as IDictionary<string, object>;
            if (dictionary == null) return new RouteValueDictionary(routeValues);
            return new RouteValueDictionary(dictionary);
        }
        public static RouteValueDictionary ToRouteValueDictionary(this object routeValues, string controller, string action)
        {
            var routeValue = routeValues.ToRouteValueDictionary();
            if (!routeValue.ContainsKey("controller"))
                routeValue.Add("controller", controller);
            if (!routeValue.ContainsKey("action"))
                routeValue.Add("action", action);
            return routeValue;
        }
    }

}
