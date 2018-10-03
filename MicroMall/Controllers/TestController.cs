using Ecard.Services;
using MicroMall.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class TestController : Controller
    {
        private readonly IRebateService IRebateService;

        public TestController(IRebateService IRebateService)
        {
            this.IRebateService = IRebateService;
        }
        public ActionResult OrderRebate(int orderId)
        {
           var item=IRebateService.Rebate(orderId);
           return Json(new ResultMessage() { Code = 0, Msg= "成功" });
        }

    }
}
