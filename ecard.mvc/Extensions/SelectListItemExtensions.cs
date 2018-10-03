using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ecard.Models;
using Moonlit;
using Oxite.Model;
using System.Web;

namespace Oxite.Mvc.Extensions
{

    public static class SelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<IdNamePair> pairs, bool hasAll)
        {
            var query = pairs.Select(x => new SelectListItem() { Text = x.Name, Value = x.Key.ToString() });
            if (hasAll)
                return new List<SelectListItem> { new SelectListItem { Text = "全部", Value = "0" } }.Union(query);
            return query;
        }
    }
}
