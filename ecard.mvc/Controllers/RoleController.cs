using System;
using System.Linq;
using System.Security;
using System.Text;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Infrastructure;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.Roles;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Ecard.Infrastructure;
using System.Web.Script.Serialization;

namespace Ecard.Mvc.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IUnityContainer _container;

        public RoleController(IUnityContainer container)
        {
            _container = container;
        }
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Create()
        {
            var createRole = _container.Resolve<CreateRole>();
            createRole.Ready();
            var model = new EcardModelItem<CreateRole>(createRole);
            return View(model);
        }


        [HttpPost]
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Create([Bind(Prefix = "Item")]CreateRole item)
        {
            IMessageProvider msg = null;
            if (ModelState.IsValid(item))
            {
                this.ModelState.Clear();
                msg = item.Create();
                item = _container.Resolve<CreateRole>();
            }
            item.Ready();
            return View(new EcardModelItem<CreateRole>(item, msg));
        }
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Edit(int id)
        {
            var model = _container.Resolve<EditRole>();
            model.Read(id);
            model.Ready();
            return View(new EcardModelItem<EditRole>(model));
        }
        [HttpPost]
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Edit([Bind(Prefix = "Item")]EditRole model)
        {
            if (ModelState.IsValid)
            {
                this.ModelState.Clear();
                model.Save();
                return RedirectToAction("List");
            }
            model.Ready();
            return View(new EcardModelItem<EditRole>(model));
        }

        [HttpPost]
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Deletes(string strIds, ListRoles request)
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
            result.CodeText = "删除成功" + successCount + "个角色,失败" + errorCount + "个";
            return Json(result);
        }
        //停用角色
        [HttpPost]
        [CheckPermission(Permissions.RoleSuspend)]
        public ActionResult Suspend(int id, ListRoles request)
        {
            return Json(request.Suspend(id));
        }

        [HttpPost]
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Suspends(string strIds, ListRoles request)
        {
            ResultMsg result=new ResultMsg();
            int successCount = 0;
            int errorCount = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
               string[] sId=  strIds.Split(',');
               foreach (var id in sId)
               {
                   int intId = 0;
                   if (int.TryParse(id, out intId))
                   {
                      result= request.Suspend(intId);
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
            result.CodeText = "停用成功"+successCount+"个角色,失败"+errorCount+"个"; 
            return Json(result);
        }
        // 启用角色
        [HttpPost]
        [CheckPermission(Permissions.RoleSume)]
        public ActionResult Resume(int id, ListRoles request)
        {
            return Json(request.Resume(id));
        }

        [HttpPost]
        [CheckPermission(Permissions.RoleEdit)]
        public ActionResult Resumes(string strIds, ListRoles request)
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
            result.CodeText = "启用成功" + successCount + "个角色,失败" + errorCount + "个";
            return Json(result);
        }
        //删除
        [HttpPost]
        [CheckPermission(Permissions.RoleDelete)]
        public ActionResult Delete(int id, ListRoles request)
        {
            return Json(request.Delete(id));
        }
        [CheckPermission(Permissions.Role)]
        //[DashboardItem]
        public virtual ActionResult List(ListRoles request)
        {
            string pageHtml = string.Empty;
            if (ModelState.IsValid)
            {
                request.Query(out pageHtml);
                ViewBag.pageHtml = MvcHtmlString.Create(pageHtml); 
            } 
            return View("List", request);
        }
        [HttpPost]
        [CheckPermission(Permissions.Role)]
        public ActionResult ListPost(RoleRequest request)
        {
            var createRole = _container.Resolve<ListRoles>(); 
            string pageHtml = string.Empty;
            var datas = createRole.AjaxGet(request, out pageHtml); 
            return Json(new { tables = datas, html = pageHtml });
        }
    }
}