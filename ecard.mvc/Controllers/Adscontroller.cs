using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Ads;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ecard.Requests;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.ViewModels;

namespace Ecard.Mvc.Controllers
{
    public class Adscontroller : Controller
    {

        private readonly IUnityContainer _unityContainer;
        public Adscontroller(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [Dependency, NoRender]
        public IAdService adService { get; set; }

        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }

        [CheckPermission(Permissions.Ads)]
        [Ecard.Services.DashboardItem]
        public ActionResult AdsList(AdsLists request)
        {
            request.Query();
            return View(request);
        }
        [HttpPost]
        [CheckPermission(Permissions.Ads)]
        public ActionResult AjaxList(AdRequest request)
        {
            var create = _unityContainer.Resolve<AdsLists>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

       [CheckPermission(Permissions.AdsCreate)]
        public ActionResult Create() 
        {
            CreateAds home = _unityContainer.Resolve<CreateAds>();
            var model = new EcardModelItem<CreateAds>(home);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CheckPermission(Permissions.AdsCreate)]
        public ActionResult Create(CreateAds ads)
        {

            return Json(ads.Create());
        }
        [CheckPermission(Permissions.AdsEdit)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditAds>();
            model.Ready(id);
            return View(model);
        }


        [HttpPost]
        [CheckPermission(Permissions.AdsEdit)]
        public ActionResult Edit(EditAds model)
        {
            return Json(model.Save());
        }
        /// <summary>
        /// 删除广告
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.AdsDelete)]
        public ActionResult Delete(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = adService.GetById(id);
            if (comm != null)
            {
                adService.Delete(comm);
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
        [CheckPermission(Permissions.AdsDelete)]
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
                    var comm = adService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += adService.Delete(comm);
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
                result.CodeText = "请选中您要删除的广告！";
                return Json(result);
            }

        }

        /// <summary>
        /// 检验广告状态
        /// </summary>
        /// <param name="strIds"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public ActionResult CheckAdsSate(string strIds, int State)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                if (State == AdStates.soldOut)
                {
                    var commodityIds = strIds.Split(',');
                    for (int i = 0; i < commodityIds.Length; i++)
                    {
                        var comm = adService.GetById(Convert.ToInt32(commodityIds[i]));
                        if (comm != null && comm.State == AdStates.putaway)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的广告有" + sum + "件已经上架！";
                        return Json(result);
                    }
                }
                if (State == CommodityStates.putaway)
                {
                    var commodityIds = strIds.Split(',');
                    for (int i = 0; i < commodityIds.Length; i++)
                    {
                        var comm = adService.GetById(Convert.ToInt32(commodityIds[i]));
                        if (comm != null && comm.State == AdStates.soldOut)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的广告有" + sum + "件已经下架！";
                        return Json(result);
                    }
                }

            }

            if (string.IsNullOrEmpty(strIds) && State == AdStates.soldOut)
            {
                result.Code = 1;
                result.CodeText = "请选中您要上架的广告！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(strIds) && State == AdStates.putaway)
            {
                result.Code = 1;
                result.CodeText = "请选中您要下架的广告！";
                return Json(result);
            }
            else
            {
                result.Code = 2;
                return Json(result);
            }
        }

        /// <summary>
        /// 广告批量下架
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.AdsSoldout)]
        public ActionResult Soldout(string strIds)
        {

            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = adService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.State = AdStates.soldOut;
                    try
                    {
                        sum += adService.Update(comm);
                    }
                    catch (Exception)
                    {
                        result.Code = 2;
                        result.CodeText = "不好意思,系统异常!";
                        return Json(result);
                    }

                }
                transaction.Commit();
                if (sum == commodityIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = sum + "个广告下架成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "批量下架广告失败!";
                }
                return Json(result);
            }
            else
            {
                result.Code = 2;
                result.CodeText = "请选中您要下架的广告！";
                return Json(result);
            }
        }
        /// <summary>
        /// 广告批量上架
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.AdsPutaway)]
        public ActionResult Putaway(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = adService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.State = AdStates.putaway;
                    try
                    {
                        sum += adService.Update(comm);
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
                    result.CodeText = sum + "个广告上架成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "批量上架广告失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要上架的广告！";
                return Json(result);
            }
        }
    }
}
