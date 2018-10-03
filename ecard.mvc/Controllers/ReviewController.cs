using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Reviews;
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
    public class ReviewController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        [Dependency]
        [NoRender]
        public IReviewService ReviewService { get; set; }
        public ReviewController(IUnityContainer unityContainer) 
        {
            _unityContainer = unityContainer;
        }
        [CheckPermission(Permissions.ListReview)]
        public ActionResult List(ListReviews request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        public ActionResult AjaxList(ReviewRequest request)
        {
            var create = _unityContainer.Resolve<ListReviews>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        /// <summary>
        ///批量显示
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.ListReview)]
        public ActionResult ReviewShow(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = ReviewService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.State = ReviewStates.Show; //CommodityStates.putaway;
                    try
                    {
                        sum += ReviewService.Update(comm);
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
                    result.CodeText = sum + "条评论审核成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "审核失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要审核的评论！";
                return Json(result);
            }
        }

        /// <summary>
        ///批量不显示
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.ListReview)]
        public ActionResult ReviewNotShow(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = ReviewService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.State = ReviewStates.NotShow; //CommodityStates.putaway;
                    try
                    {
                        sum += ReviewService.Update(comm);
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
                    result.CodeText = sum + "条评论审核成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "审核失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要审核的评论！";
                return Json(result);
            }
        }

       
    }
}
