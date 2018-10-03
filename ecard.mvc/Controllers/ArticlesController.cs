using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Articles;
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
    public class ArticlesController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IArticlesService IArticlesService;
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }
        public ArticlesController(IUnityContainer unityContainer, IArticlesService IArticlesService)
        {
            _unityContainer = unityContainer;
            this.IArticlesService = IArticlesService;
        }

        [CheckPermission(Permissions.ArticlesCreate)]
        public ActionResult Create()
        {
            CreateArticles home = _unityContainer.Resolve<CreateArticles>();
            var model = new EcardModelItem<CreateArticles>(home);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string title, string describe)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入标题" });
                if (string.IsNullOrWhiteSpace(describe))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入详情" });
                var model = new Articles();
                model.describe = describe;
                model.title = title;
                model.submitTime = DateTime.Now;
                if (IArticlesService.Insert(model) > 0)
                    return Json(new ResultMsg() { Code = 0 });
                return Json(new ResultMsg() { Code = -1, CodeText = "发布失败" });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = "系统错误，联系管理员" });
            }
        }
        [CheckPermission(Permissions.ListArticles)]
        public ActionResult List(ListArticles request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        [CheckPermission(Permissions.ListArticles)]
        public ActionResult AjaxList(ArticlesRequest request)
        {
            var create = _unityContainer.Resolve<ListArticles>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
        [CheckPermission(Permissions.EditArticles)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditArticles>();
            model.Ready(id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CheckPermission(Permissions.EditArticles)]
        public ActionResult Edit(string title, string describe, int articleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入标题" });
                if (string.IsNullOrWhiteSpace(describe))
                    return Json(new ResultMsg() { Code = -1, CodeText = "请输入详情" });
                var model = IArticlesService.GetById(articleId);
                model.describe = describe;
                model.title = title;
                model.submitTime = DateTime.Now;
                if (IArticlesService.Update(model) > 0)
                    return Json(new ResultMsg() { Code = 0 });
                return Json(new ResultMsg() { Code = -1, CodeText = "保存失败" });
            }
            catch (Exception ex)
            {
                return Json(new ResultMsg() { Code = -1, CodeText = "系统错误，联系管理员" });
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.DeleteArticles)]
        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = IArticlesService.GetById(id);
            if (comm != null)
            {
                IArticlesService.Delete(comm);
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
        /// 批量删除
        /// </summary>
        /// <param name="strIds"></param>
        /// <returns></returns>
        [CheckPermission(Permissions.DeleteArticles)]
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
                    var comm = IArticlesService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += IArticlesService.Delete(comm);
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
                result.CodeText = "请选中您要删除的文章！";
                return Json(result);
            }

        }
    }
}
