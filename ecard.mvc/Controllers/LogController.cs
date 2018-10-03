using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.Logs;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        private readonly IUnityContainer _unityContainer;

        public LogController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [CheckPermission(Permissions.Log)]
        //[DashboardItem]
        public virtual ActionResult List(ListLogs request)
        {
             string pageHtml = string.Empty;
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                request.Query(out pageHtml);
                ViewBag.pageHtml = MvcHtmlString.Create(pageHtml);
            }
            request.Ready();
            return View("List", request);
        }
        [CheckPermission(Permissions.Log)]
        [HttpPost]
        public ActionResult ListPost(LogRequest request)
        {
            var createRole = _unityContainer.Resolve<ListLogs>();
            string pageHtml = string.Empty;
            var datas = createRole.AjaxGet(request,out pageHtml);
            return Json(new { tables = datas, html = pageHtml });
        }
    }
}
