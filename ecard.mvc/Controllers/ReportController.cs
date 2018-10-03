using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Report;
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
    public class ReportController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        public ReportController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [Dependency, NoRender]
        public ICommoditySalesService ICommoditySalesService { get; set; }
        [CheckPermission(Permissions.ListCommoditySales)]
        public ActionResult ListCommoditySales(ListCommoditySales request)
        {
           request.Query();
           return View(request);
        }
        [CheckPermission(Permissions.ListCommoditySales)]
        [HttpPost]
        public ActionResult AjaxCommoditySales(CommoditySalesStatisticsRequest request)
        {
            var create = _unityContainer.Resolve<ListCommoditySales>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }
    }
}
