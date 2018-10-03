using Ecard.Services;
using MicroMall.Models;
using MicroMall.Models.Withdraws;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class WithdrawController : Controller
    {
       
         private readonly IUnityContainer _container;

        private readonly ILog4netService Log4netService;

        public WithdrawController(IUnityContainer container, ILog4netService log4netService)
        {
            _container = container;
            Log4netService = log4netService;
            //_loadIndex =loadIndex;
            //CommodityService = commodityService;
        }

        public ActionResult Withdraw()
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return RedirectToAction("Index", "login");
            var request = _container.Resolve<OperationWithdraw>();
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            request.Ready(userId);
            return View(request);
        }
        [HttpPost]
        public ActionResult AddWithdraw(OperationWithdraw request)
        {
            if (Request.Cookies[SessionKeys.USERID] == null)
                return Json(new ResultMessage() { Code = -1, Msg = "请重新登录" });
            int userId = Convert.ToInt32(Request.Cookies[SessionKeys.USERID].Value);
            return Json(request.Save(userId));
        }

        public ActionResult List(ListWithdraws request)
        {
            request.Query(1);
            return View(request);
        }
        [HttpPost]
        public ActionResult AjaxList(int pageIndex)
        {
            var request = _container.Resolve<ListWithdraws>();
            var list = request.Query(pageIndex);
            return Json(list);
        }

    }
}
