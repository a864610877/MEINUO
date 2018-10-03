using Ecard.Services;
using MicroMall.Models;
using MicroMall.Models.Commoditys;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class CommodityController : Controller
    {
        //
        // GET: /Commodity/

        private readonly IUnityContainer _container;

        private readonly ILog4netService Log4netService;

        private readonly ICommodityService CommodityService;

        public CommodityController(IUnityContainer container, ILog4netService log4netService, ICommodityService CommodityService)
        {
            _container = container;
            Log4netService = log4netService;
            this.CommodityService = CommodityService;
            //_loadIndex =loadIndex;
            //CommodityService = commodityService;
        }
        [HttpGet]
        public ActionResult GetCommodity(int start,int count)
        {
            var request = _container.Resolve<OperationCommodity>();
            request.Url();
            return Json(request.GetCommodity(start, count),JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult CommodityDetail(int id)
        {
            var request = _container.Resolve<OperationCommodity>();
            request.Load();
            request.Ready(id);
            return View("CommodityDetail", request);
        }
        [HttpPost]
        public ActionResult CommodityDetailJosn(int id)
        {
            var request = _container.Resolve<OperationCommodity>();
           var data= request.GetByCommodity(id);
           return Json(data);
        }

        public ActionResult AddReview(int commodityId, string orderNo)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            var model = CommodityService.GetById(commodityId);
            ViewData["orderNo"] = orderNo;
            return View(model);
        }

        [HttpPost]
        public ActionResult AddReview(string content, int commodityId, string orderNo)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
            int userId = 0;
            int.TryParse(strId, out userId);
            var request = _container.Resolve<OperationCommodity>();
            return Json(request.AddReview(content, commodityId, orderNo,userId));
        }
        [HttpPost]
        public ActionResult DeleteReview(int ReviewId)
        {
            var request = _container.Resolve<OperationCommodity>();
            return Json(request.DeleteReview(ReviewId));
        }
        [HttpPost]
        public ActionResult GetReview(int commodityId,int pageIndex)
        {
            var request = _container.Resolve<OperationCommodity>();
            return Json(request.GetReview(commodityId, pageIndex));
        }


    }
}
