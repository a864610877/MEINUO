using MicroMall.Models.Regists;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class RegistsController : Controller
    {
        private readonly IUnityContainer _container;
        public RegistsController(IUnityContainer container)
        {
            _container = container;
        }

        public ActionResult Regist(string orangeKey)
        {
            var request = _container.Resolve<RegistRequest>();
            request.orangeKey = orangeKey;
            return View(request);
        }

        [HttpPost]
        public ActionResult Regist(RegistModel model)
        {
            var request = _container.Resolve<RegistRequest>();
            request.Set(model);
            return Json(request.Save());
        }
    }
}