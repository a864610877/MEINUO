using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.SecondKillCommodityss;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class SecondKillCommoditysController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        public SecondKillCommoditysController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult List(ListSecondKillCommodityss request)
        {
            request.Query();
            return View(request);
        }

        [HttpPost]
        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult AjaxList(SecondKillCommoditysRequest request)
        {
            var create = _unityContainer.Resolve<ListSecondKillCommodityss>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Create()
        {
            var createAccountType = _unityContainer.Resolve<AddSecondKillCommodity>();
            var model = new EcardModelItem<AddSecondKillCommodity>(createAccountType);
            return View(model);
        }
        [HttpPost]
        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Create([Bind(Prefix = "Item")] AddSecondKillCommodity model)
        {
            IMessageProvider msg = null;
            if (ModelState.IsValid(model))
            {
                this.ModelState.Clear();

                msg = model.Create();
                model = _unityContainer.Resolve<AddSecondKillCommodity>();
            }
            return View(new EcardModelItem<AddSecondKillCommodity>(model, msg));
        }

        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditSecondKillCommodity>();
            model.Ready(id);
            return View(new EcardModelItem<EditSecondKillCommodity>(model));
        }


        [HttpPost]
        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Edit([Bind(Prefix = "Item")] EditSecondKillCommodity model)
        {
            if (ModelState.IsValid(model))
            {
                this.ModelState.Clear();
                model.Save();
            }
            model.Ready(model.Id);
            return View(new EcardModelItem<EditSecondKillCommodity>(model));
        }

        [HttpPost]
        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Deletes(string strIds, ListSecondKillCommodityss request)
        {
            ResultMsg result = new ResultMsg();
            int successCount = 0;
            int errorCount = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                string[] sId = strIds.Split(',');
                foreach (var id in sId)
                {
                    int intId = 0;
                    if (int.TryParse(id, out intId))
                    {
                        result = request.Delete(intId);
                        if (result.Code == 1)
                        {
                            successCount += 1;
                        }
                        else
                        {
                            errorCount += 1;
                        }
                    }
                }
            }
            result.CodeText = "删除成功" + successCount + "个用户,失败" + errorCount + "个";
            return Json(result);
        }

        [HttpPost]
        [CheckPermission(Permissions.ManageSecondKillCommoditys)]
        public ActionResult Delete(int id, ListSecondKillCommodityss request)
        {
            return Json(request.Delete(id));
        }
    }
}
