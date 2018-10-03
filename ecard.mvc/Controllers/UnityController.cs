using System.Web.Mvc;
using Ecard.Mvc.Models;

namespace Ecard.Mvc.Controllers
{
    public class UnityController : Controller
    {
        public ActionResult Print()
        {
            return Content("");
        }
    }
}