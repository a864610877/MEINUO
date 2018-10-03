using Ecard.Services;
using MicroMall.Models.Homes;
using MicroMall.Models.ImageAds;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroMall.Controllers
{
    public class ImageAdsController : Controller
    {
        //商城首页滚动横幅广告
        // GET: /ImageAds/
        private readonly IUnityContainer _container;
        private readonly ILog4netService Log4netService;
        private ISiteService ISiteService { get; set; }
        string url = ""; //System.Configuration.ConfigurationManager.AppSettings["adminUrl"];
        public ImageAdsController(IUnityContainer _container, ILog4netService Log4netService, ISiteService ISiteService)
        {
            this._container = _container;
            this.Log4netService = Log4netService;
            this.ISiteService = ISiteService;
            var site = ISiteService.Query(null).FirstOrDefault();
            if (site != null)
                url = site.imageUrl;

        }

        [HttpPost]
        public ActionResult Index()
        {
            var request = _container.Resolve<OperationImageAds>();
            var model = new ImageAdsListModel();
            var recordSet = request.ImageAdsService.Query();
            if (recordSet != null)
            {
                model.AdsList = recordSet.ToList().Select(x => new ImageAdsModel()
                {
                    adId = x.adId,
                    ImageUrl = url+x.ImageUrl,
                    link = x.link
                }).ToList();
            }

            return Json(model);
        }

    }
}
