using System.Web.Mvc;
using Ecard.Mvc.Models.Homes;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnityContainer _container;

        public HomeController(IUnityContainer container)
        {
            _container = container;
        }

        public ActionResult Index()
        { 
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            DashboardHome home = _container.Resolve<DashboardHome>();
            home.Ready();
            return View(home);
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult UserError()
        {
            return View();
        }
    }
}
