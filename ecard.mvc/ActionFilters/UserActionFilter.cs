//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Moonlit.Collections;

namespace Ecard.Mvc.ActionFilters
{
    /// <summary>
    /// 负责 对 oxiteModel 的用户进行处理，包括处理 model.User 和 currentUser 参数
    /// </summary>
    public class UserActionFilter : IActionFilter
    {
        private readonly SecurityHelper _securityHelper;

        public UserActionFilter(SecurityHelper securityHelper)
        {
            _securityHelper = securityHelper;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel;

            if (model != null)
            {
                var user = filterContext.HttpContext.User.Identity.IsAuthenticated
                    ? _securityHelper.GetCurrentUser()
                    : null;

                if (user != null)
                {
                    model.User = user.CurrentUser;
                    model.UserModel = user;
                }
            }
        }



        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("currentUser"))
            {
                UserModel userModel = null;

                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                    userModel = _securityHelper.GetCurrentUser();
                if (userModel != null)
                    filterContext.ActionParameters["currentUser"] = userModel.CurrentUser;
            }
        }
    }
    public class AdminUserModel : UserModel
    {
        private readonly AdminUser _adminUser;

        public AdminUserModel(AdminUser adminUser)
        {
            _adminUser = adminUser;

            Roles = adminUser.Roles.Select(x => x.DisplayName).ToList();
        }

        public string Name
        {
            get { return InnerObject.Name; }
        }

        public DateTime? LastSignInTime
        {
            get { return InnerObject.LastSignInTime; }
        }
        public string DisplayName
        {
            get { return InnerObject.DisplayName; }
        }

        public List<string> Roles { get; set; }
        protected AdminUser InnerObject
        {
            get { return _adminUser; }
        }

        public override User CurrentUser
        {
            get { return _adminUser; }
        }
    }
}
