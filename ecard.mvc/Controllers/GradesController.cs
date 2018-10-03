using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Grades;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class GradesController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        public GradesController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [Dependency, NoRender]
        public IGradesService IGradesService { get; set; }

        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }
        [CheckPermission(Permissions.ListGrades)]
        public ActionResult List(ListGrades request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        [CheckPermission(Permissions.ListGrades)]
        public ActionResult AjaxList(GradesRequest request)
        {
            var create = _unityContainer.Resolve<ListGrades>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
        [CheckPermission(Permissions.GradesCreate)]
        public ActionResult Create()
        {
            CreateGrades home = _unityContainer.Resolve<CreateGrades>();
            var model = new EcardModelItem<CreateGrades>(home);
            return View(model);
        }
        [HttpPost]
        [CheckPermission(Permissions.GradesCreate)]
        public ActionResult Create(string name, decimal sale)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入名称" });

                var model = new Grades();
                model.name = name;
                model.sale = sale;
                if (IGradesService.Insert(model) > 0)
                    return Json(new ResultMsg() { Code = 0 });
                return Json(new ResultMsg() { Code = -1, CodeText = "添加失败" });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = "系统错误，联系管理员" });
            }
        }
         [CheckPermission(Permissions.EditGrades)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditGrades>();
            model.Ready(id);
            return View(model);
        }
        [HttpPost]
        [CheckPermission(Permissions.EditGrades)]
        public ActionResult Edit(EditGrades request)
        {
            return Json(request.Save());
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.DeleteGrades)]
        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = IGradesService.GetById(id);
            if (comm != null)
            {
                IGradesService.Delete(comm);
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

        /// <summary>
        /// 批量删除广告
        /// </summary>
        /// <param name="strIds"></param>
        /// <returns></returns>
        [CheckPermission(Permissions.DeleteGrades)]
        public ActionResult Deletes(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = IGradesService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += IGradesService.Delete(comm);
                    }
                    catch (Exception)
                    {
                        result.CodeText = "不好意思,系统异常!";
                        return Json(result);
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
                result.CodeText = "请选中您要删除的级别！";
                return Json(result);
            }

        }

    }
}
