using Ecard.Services;
using MicroMall.Models;
using MicroMall.Models.Homes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IUnityContainer _container;

        private readonly ILog4netService Log4netService;
        private readonly ISiteService ISiteService;

        //private readonly LoadIndex _loadIndex;
        //[Dependency]
        //private readonly ICommodityService CommodityService;

        private IArticlesService IArticlesService;
        public HomeController(IUnityContainer container, ILog4netService log4netService, IArticlesService IArticlesService, ISiteService ISiteService)
        {
            _container = container;
            Log4netService = log4netService;
            this.IArticlesService = IArticlesService;
            this.ISiteService = ISiteService;
            //_loadIndex =loadIndex;
            //CommodityService = commodityService;
        }
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //Log4netService.Insert("123213");
            var request = _container.Resolve<LoadIndex>();
            request.Ready();
            return View("Index",request);
        }

        [HttpPost]
        public ActionResult GetCommodity(int indexPage,string content)
        {
            var _loadIndex = _container.Resolve<LoadIndex>();
            _loadIndex.Url();
            return Json(_loadIndex.GetCommodity(indexPage, content));
        }

        [HttpPost]
        public ActionResult Article(int id)
        {
            string url = "";
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site != null)
                url = site.adminUrl;
            var item = IArticlesService.GetById(id);
            if (item != null)
            {
                item.describe = item.describe.Replace("/MicroMalls/CommodityImages", url + "/MicroMalls/CommodityImages");
            }
            return Json(item);
        }
    }
}
