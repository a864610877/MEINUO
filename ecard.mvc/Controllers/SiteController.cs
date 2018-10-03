using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Sites;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Ecard.Models.Sites;

namespace Ecard.Mvc.Controllers
{
    [Authorize]
    public class SiteController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        private readonly Site _site;

        public SiteController(IUnityContainer unityContainer, Site site)
        {
            _unityContainer = unityContainer;
            _site = site;
        }

        [CheckPermission(Permissions.SystemSettings)]
        public ActionResult EditSite()
        {
            var model = _unityContainer.Resolve<EditSite>();
            model.Ready();
            return View(new EcardModelItem<EditSite>(model));
        } 

        [HttpPost]
        [CheckPermission(Permissions.SystemSettings)]
        public ActionResult EditSite([Bind(Prefix = "Item")] EditSite model)
        {
            if (ModelState.IsValid)
            {
                this.ModelState.Clear();
                model.Save();
                model.Ready();
            }
            return View(new EcardModelItem<EditSite>(model));
        }
        [CheckPermission(Permissions.SmsSeting)]
        public ActionResult SmsParam()
        {
            var model = _unityContainer.Resolve<SmsParam>();
            model.Ready();
            return View(new EcardModelItem<SmsParam>(model));
        }
        [HttpPost]
        [CheckPermission(Permissions.SmsSeting)]
        public ActionResult SmsParam([Bind(Prefix = "Item")] SmsParam model)
        {
            if (ModelState.IsValid)
            {
                this.ModelState.Clear();
                model.Save();
                model.Ready();
            }
            return View(new EcardModelItem<SmsParam>(model));
        }
    }
}
