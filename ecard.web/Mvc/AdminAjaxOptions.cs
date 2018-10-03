using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;

namespace Ecard.Mvc
{
    public class AdminAjaxOptions : AjaxOptions
    {
        public AdminAjaxOptions()
        {
            this.UpdateTargetId = "mainPanel";
        }
    }
}