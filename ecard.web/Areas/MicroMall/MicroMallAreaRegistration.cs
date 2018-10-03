using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecard.Web.Areas.MicroMall
{
    public class MicroMallAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MicroMall";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MicroMall_default",
                "MicroMall/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}