using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.SecondKillSets;
using Ecard.Mvc.ViewModels;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class SecondKillSetController : Controller
    {
        private readonly IUnityContainer _unityContainer;

        public SecondKillSetController(IUnityContainer unityContaine)
        {
            this._unityContainer = unityContaine;
        }
       // [CheckPermission(Permissions.AdmissionTicketEdit)]
        public ActionResult Edit()
        {
            var model = _unityContainer.Resolve<EditSecondKillSet>();
            model.Ready();
            return View(new EcardModelItem<EditSecondKillSet>(model));
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix = "Item")] EditSecondKillSet model)
        {
            IMessageProvider msg = null;
            if (ModelState.IsValid(model))
            {
                this.ModelState.Clear();
                msg = model.Save();
                
            }
            return View(new EcardModelItem<EditSecondKillSet>(model, msg));
        }
    }
}
