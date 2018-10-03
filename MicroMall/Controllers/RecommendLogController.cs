using MicroMall.Models.RecommendLogs;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class RecommendLogController : Controller
    {
        private readonly IUnityContainer _container;
        public RecommendLogController(IUnityContainer container)
        {
            _container = container;
        }

        public ActionResult List()
        {
            var request = _container.Resolve<ListRecommendLogs>();
                request.Load();
            return View(request);
        }

    }
}
