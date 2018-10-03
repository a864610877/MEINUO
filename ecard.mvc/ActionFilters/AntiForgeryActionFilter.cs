//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.ViewModels;

namespace Ecard.Mvc.ActionFilters
{
    /// <summary>
    /// 自动 添加防重放攻击
    /// </summary>
    public class AntiForgeryActionFilter : IActionFilter
    {
        private readonly Site site;

        public AntiForgeryActionFilter(Site site)
        {
            this.site = site;
        }

        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel;

            if (model != null)
            {
                model.AntiForgeryToken = new AntiForgeryToken(site.SiteId.ToString());
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
         
        }

        #endregion
    }
}
