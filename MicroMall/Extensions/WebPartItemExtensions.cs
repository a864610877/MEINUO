using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Oxite.Model;

namespace Oxite.Mvc.Extensions
{
    public static class WebPartItemExtensions
    {
        public static string CreateUrl(this WebPartItem webPartItem, HtmlHelper htmlHelper)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return urlHelper.RouteUrl(webPartItem.CreateRouteDictionary());
        }
    }
}
