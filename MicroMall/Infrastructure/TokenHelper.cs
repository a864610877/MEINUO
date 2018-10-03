using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Oxite.Mvc.Infrastructure
{
    class TokenHelper : ITokenHelper
    {
        public string GetCurrentToken(Controller controller)
        {
            string loginTokenOld = controller.Session["__loginToken"] as string;
            if (loginTokenOld == null)
                loginTokenOld = "123";
            return loginTokenOld;
        }

        public void CreateToken(Controller controller)
        {
            controller.Session["__loginToken"] = Guid.NewGuid().ToString("N");
            controller.ViewData["loginToken"] = BitConverter.ToString(Encoding.ASCII.GetBytes(controller.Session["__loginToken"].ToString())).Replace("-", "");
        }
    }
}
