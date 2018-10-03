using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Accounts;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUnityContainer _unityContainer;
        public AccountController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            //_securityHelper = securityHelper;
        }
        [Dependency, NoRender]
        public IAccountService accountService { get; set; }

        [Dependency, NoRender]
        public IMembershipService membershipService { get; set; }
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        [CheckPermission(Permissions.Account)]
        /// <summary>
        /// 会员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountList(ListAccounts request)
        {
            request.Query();
            return View(request);
        }
        public ActionResult AjaxList(AccountRequest request)
        {
            var create = _unityContainer.Resolve<ListAccounts>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        /// <summary>
        /// 编辑会员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckPermission(Permissions.Account)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditAccount>();
            model.Ready(id);
            return View(model);
        }
        [HttpPost]
        [CheckPermission(Permissions.Account)]
        public ActionResult Edit(EditAccount model)
        {
            return Json(model.Save());
        }



        /// <summary>
        /// 删除会员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckPermission(Permissions.Account)]
        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var account = accountService.GetById(id);
            if (account != null)
            {
                accountService.Delete(account);
                result.Code = 1;
                result.CodeText = "删除成功!";
                return Json(result);
            }
            else
            {
                result.Code = 2;
                result.CodeText = "删除失败!";
                return Json(result);
            }

        }
        [CheckPermission(Permissions.Account)]
        /// <summary>
        /// 批量停用
        /// </summary>
        /// <param name="strIds"></param>
        /// <returns></returns>
        public ActionResult BatchOutage(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var accountIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < accountIds.Length; i++)
                {
                    var account = accountService.GetById(Convert.ToInt32(accountIds[i]));
                    account.state = AccountStates.blockup;
                    var user = membershipService.GetUserById(account.userId);
                    user.State = UserStates.Invalid;
                    if (account != null && user != null)
                    {
                        try
                        {
                            sum += accountService.Update(account);
                            membershipService.UpdateUser(user);
                        }
                        catch (Exception)
                        {
                            result.CodeText = "不好意思,系统异常!";
                            return Json(result);
                        }
                    }


                }
                transaction.Commit();
                if (sum == accountIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = "停用" + sum + "个会员成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "批量停用会员失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要停用的会员！";
                return Json(result);
            }

        }

        [CheckPermission(Permissions.Account)]
        /// <summary>
        /// 批量启用
        /// </summary>
        /// <param name="strIds"></param>
        /// <returns></returns>
        public ActionResult BatchEnabled(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var accountIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < accountIds.Length; i++)
                {
                    var account = accountService.GetById(Convert.ToInt32(accountIds[i]));
                    account.state = AccountStates.normal;
                    var user = membershipService.GetUserById(account.userId);
                    user.State = UserStates.Normal;
                    if (account != null && user != null)
                    {
                        try
                        {
                            sum += accountService.Update(account);
                            membershipService.UpdateUser(user);
                        }
                        catch (Exception)
                        {
                            result.CodeText = "不好意思,系统异常!";
                            return Json(result);
                        }
                    }


                }
                transaction.Commit();
                if (sum == accountIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = "启用" + sum + "个会员成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "批量启用会员失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要启用的会员！";
                return Json(result);
            }

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.Account)]
        public ActionResult DeleteAccount(string strIds) 
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = accountService.GetById(Convert.ToInt32(commodityIds[i]));
                    if (comm!=null)
                    {
                        try
                        {

                            sum += accountService.Delete(comm);
                        }
                        catch (Exception)
                        {
                            result.CodeText = "不好意思,系统异常!";
                            return Json(result);
                        }
                    }
                }
                transaction.Commit();
                if (sum == commodityIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = "删除成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "删除失败!";
                }
                return Json(result);
            }
            else
            {
                result.Code = 2;
                result.CodeText = "请选中您要删除的商品！";
                return Json(result);
            }
        }
        /// <summary>
        /// 检验会员的状态：是否已经停用、启用
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckAccountSate(string strIds, int State)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            var accountIds = strIds.Split(',');

            if (!string.IsNullOrEmpty(strIds))
            {
                if (State == AccountStates.blockup)
                {
                    for (int i = 0; i < accountIds.Length; i++)
                    {
                        var comm = accountService.GetById(Convert.ToInt32(accountIds[i]));
                        if (comm != null && comm.state == AccountStates.blockup)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的会员有" + sum + "个已经停用！";
                        return Json(result);
                    }
                    else
                    {
                        
                    }
                }
                if (State == AccountStates.normal)
                {
                    for (int i = 0; i < accountIds.Length; i++)
                    {
                        var comm = accountService.GetById(Convert.ToInt32(accountIds[i]));
                        if (comm != null && comm.state == AccountStates.normal)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的会员有" + sum + "个已经启用！";
                        return Json(result);
                    }
                }

            }

            if (string.IsNullOrEmpty(strIds) && State == AccountStates.blockup)
            {
                result.Code = 1;
                result.CodeText = "请选中您要停用的会员！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(strIds) && State == AccountStates.normal)
            {
                result.Code = 1;
                result.CodeText = "请选中您要启用的会员！";
                return Json(result);
            }
            else
            {
                result.Code = 2;
                return Json(result);
            }
        }

        [HttpPost]
        public ActionResult GetSaleAmount(int Id)
        {
            var amount = accountService.SaleAmount(Id,0);
            return Json(new ResultMsg() { Code = 0, CodeText = amount.ToString() });
          
        }
    }
}
