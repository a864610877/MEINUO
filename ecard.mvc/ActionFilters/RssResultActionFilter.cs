//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Web.Mvc;
using Ecard.Mvc.Results;
using Ecard.Mvc.ViewModels;
using Oxite.Mvc.Results;

namespace Ecard.Mvc.ActionFilters
{
    public class RssResultActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            object model = filterContext.Controller.ViewData.Model;

            if(model.GetType().GetGenericTypeDefinition() == typeof(EcardModelList<>))
            {
                object list = model.GetType().GetProperty("List").GetValue(model, null);

                int count = (int)list.GetType().GetProperty("Count").GetValue(list, null);

                filterContext.Result = new FeedResult("Rss", count == 0);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
