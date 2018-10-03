using Ecard.Infrastructure;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.RecommendLogs;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class RecommendLogController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        public RecommendLogController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            //_securityHelper = securityHelper;
        }

        [Dependency, NoRender]
        public IRecommendLogService recommendLogService { get; set; }

        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

         [CheckPermission(Permissions.RecommendLog)]
        public ActionResult RecommendLogList(RecommendLogLists request) 
        {
            request.Query();
            return View(request);
        }
        public ActionResult AjaxList(MemberRecommendLogRequest request)
        {
            var create = _unityContainer.Resolve<RecommendLogLists>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
        public ActionResult Edit()
        {
            return Json(1);
        }
        /// <summary>
        /// 删除推荐记录
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.RecommendLogDelete)]
        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = recommendLogService.GetById(id);
            if (comm != null)
            {
                recommendLogService.Delete(comm);
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

         [CheckPermission(Permissions.RecommendLogDelete)]
        /// <summary>
        /// 批量删除推荐记录
        /// </summary>
        /// <param name="strIds"></param>
        /// <returns></returns>
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
                    var comm = recommendLogService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += recommendLogService.Delete(comm);
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
                result.CodeText = "请选中您要删除的记录！";
                return Json(result);
            }

        }
    }
}
