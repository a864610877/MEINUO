//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Linq;
using System.Security;
using System.Text;
using System.Web.Mvc;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Infrastructure;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.AdminUsers;
using Ecard.Mvc.Models.Roles;
using Ecard.Mvc.Models.Users;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Oxite.Validation;
using System;

namespace Ecard.Mvc.Controllers
{
    [Authorize]
    public class AdminUserController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly IUnityContainer _unityContainer;
        private readonly LogHelper _logger;
       
         public AdminUserController(IUnityContainer container)
        {
            _unityContainer = container;
        }
        public AdminUserController(IMembershipService membershipService, IUnityContainer unityContainer, LogHelper logger)
        {
            _membershipService = membershipService;
            _unityContainer = unityContainer;
            _logger = logger;
        }


        #region AdminUser
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Create()
        {
            var createAdminUser = _unityContainer.Resolve<CreateAdminUser>();
            createAdminUser.Ready();
            var model = new EcardModelItem<CreateAdminUser>(createAdminUser);
            return View(model);
        }


        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Create([Bind(Prefix = "Item")]CreateAdminUser user)
        {
            IMessageProvider messages = null;
            if (ModelState.IsValid(user))
            {
                this.ModelState.Clear();

                messages = user.Save();
                user = _unityContainer.Resolve<CreateAdminUser>();
            }
            user.Ready();
            return View(new EcardModelItem<CreateAdminUser>(user, messages));
        }
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditAdminUser>();
            model.Read(id);
            model.Ready();
            return View(new EcardModelItem<EditAdminUser>(model));
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Edit([Bind(Prefix = "Item")]EditAdminUser userModel)
        {
            var user = userModel.InnerObject;
            if (ModelState.IsValid)
            {
                this.ModelState.Clear();
                userModel.Save();
                return RedirectToAction("List");
            }
            userModel.Roles.Merge(_membershipService.QueryRoles(new RoleRequest()).Select(x => new ListRole(x)));
            return View(new EcardModelItem<EditAdminUser>(userModel));
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Deletes(string strIds, ListAdminUsers request)
        {
            ResultMsg result = new ResultMsg();
            int successCount = 0;
            int errorCount = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                string[] sId = strIds.Split(',');
                foreach (var id in sId)
                {
                    int intId = 0;
                    if (int.TryParse(id, out intId))
                    {
                        result = request.Delete(intId);
                        if (result.Code == 1)
                        {
                            successCount += 1;
                        }
                        else
                        {
                            errorCount += 1;
                        }
                    }
                }
            }
            result.CodeText = "删除成功" + successCount + "个用户,失败" + errorCount + "个";
            return Json(result);
        }
        //停用
        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Suspend(int id, ListAdminUsers request)
        { 
            return Json(request.Suspend(id));
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Suspends(string strIds, ListAdminUsers request)
        {
            ResultMsg result = new ResultMsg();
            int successCount = 0;
            int errorCount = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                string[] sId = strIds.Split(',');
                foreach (var id in sId)
                {
                    int intId = 0;
                    if (int.TryParse(id, out intId))
                    {
                        result = request.Suspend(intId);
                        if (result.Code == 1)
                        {
                            successCount += 1;
                        }
                        else
                        {
                            errorCount += 1;
                        }
                    }
                }
            }
            result.CodeText = "停用成功" + successCount + "个用户,失败" + errorCount + "个";
            return Json(result);
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Resume(int id, ListAdminUsers request)
        {
            return Json(request.Resume(id));
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Resumes(string strIds, ListAdminUsers request)
        {
            ResultMsg result = new ResultMsg();
            int successCount = 0;
            int errorCount = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                string[] sId = strIds.Split(',');
                foreach (var id in sId)
                {
                    int intId = 0;
                    if (int.TryParse(id, out intId))
                    {
                        result = request.Resume(intId);
                        if (result.Code == 1)
                        {
                            successCount += 1;
                        }
                        else
                        {
                            errorCount += 1;
                        }
                    }
                }
            }
            result.CodeText = "启用成功" + successCount + "个用户,失败" + errorCount + "个";
            return Json(result);
        }

        [HttpPost]
        [CheckPermission(Permissions.UserEdit)]
        public ActionResult Delete(int id, ListAdminUsers request)
        {
            return Json(request.Delete(id));
        }
        [CheckPermission(Permissions.User)]
        
        public ActionResult Export(ListAdminUsers request)
        {
            _logger.LogWithSerialNo(LogTypes.AdminUserExport, SerialNoHelper.Create(), 0);
            return List(request);
        }

        /// <summary>
        /// 获取用户表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [CheckPermission(Permissions.User)]
        //[DashboardItem]
        public ActionResult List(ListAdminUsers request)
        {
            string pageHtml = string.Empty;
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                request.Query( out pageHtml);
                ViewBag.pageHtml = MvcHtmlString.Create(pageHtml);
            }
            return View("List", request);
        }
        [CheckPermission(Permissions.User)]
        [HttpPost]
        public ActionResult List(UserRequest request)
        {

            var createRole = _unityContainer.Resolve<ListAdminUsers>();
            string pageHtml = string.Empty;
            var datas = createRole.AjaxGet(request, out pageHtml);
            return Json(new { tables = datas, html = pageHtml });
        }
        #endregion

    }
}
