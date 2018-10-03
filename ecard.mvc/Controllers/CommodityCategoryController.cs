using Ecard.Models;
using Ecard.Mvc.Models.CommodityCategory;
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
    public class CommodityCategoryController : Controller
    {
         private readonly IUnityContainer _unityContainer;
         public CommodityCategoryController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [Dependency, NoRender]
         public ICommodityCategorysService ICommodityCategorysService { get; set; }
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        //[CheckPermission(Permissions.ListGrades)]
        public ActionResult List(ListCommodityCategorys request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        public ActionResult AjaxList(CommodityCategorysRequest request)
        {
            var create = _unityContainer.Resolve<ListCommodityCategorys>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
        public ActionResult Create()
        {
            EditCommodityCategory home = _unityContainer.Resolve<EditCommodityCategory>();
            var model = new EcardModelItem<EditCommodityCategory>(home);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入名称" });

                var model = new fz_CommodityCategorys();
                model.name = name;
                model.submitTime = DateTime.Now;
                if (ICommodityCategorysService.Insert(model) > 0)
                    return Json(new ResultMsg() { Code = 0 });
                return Json(new ResultMsg() { Code = -1, CodeText = "添加失败" });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = "系统错误，联系管理员" });
            }
        }
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditCommodityCategory>();
            model.Ready(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(EditCommodityCategory request)
        {
            var item = ICommodityCategorysService.GetById(request.commodityCategoryId);
            if(item==null)
                return Json(new ResultMsg() { Code = -1, CodeText = "分类不存在" });
            item.name = request.Name;
            if (ICommodityCategorysService.Update(item) > 0)
                return Json(new ResultMsg() { Code = 0 });
            return Json(new ResultMsg() { Code = -1, CodeText = "保存失败" });
        }

        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = ICommodityCategorysService.GetById(id);
            if (comm != null)
            {
                ICommodityCategorysService.Delete(comm);
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
                    var comm = ICommodityCategorysService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += ICommodityCategorysService.Delete(comm);
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
