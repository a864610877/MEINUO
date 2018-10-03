using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.SetWeChats;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class WeChatController : Controller
    {
        private readonly IUnityContainer _unityContainer;

        public WeChatController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [Dependency]
        [NoRender]
        public ISetWeChatService setWeChatService { get; set; }

        [CheckPermission(Permissions.SetWeChat)]
        public ActionResult Edit()
        {
            var model = _unityContainer.Resolve<EditSetWeChat>();
            model.Ready();
            return View(model);
        }
        [HttpPost]
        [CheckPermission(Permissions.SetWeChat)]
        public ActionResult Edit(EditSetWeChat model)
        {
            return Json(model.Save());
        }
    }
}
