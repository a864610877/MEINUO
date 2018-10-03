using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Microsoft.Practices.Unity;
using Oxite.Infrastructure;

namespace Ecard.Mvc.ActionFilters
{
    public class SiteInfoActionFilter : IActionFilter
    {
        private readonly AppSettingsHelper appSettings;
        private readonly Site site;
        private readonly IUnityContainer _unityContainer;

        public SiteInfoActionFilter(AppSettingsHelper appSettings, Site site, IUnityContainer unityContainer)
        {
            this.appSettings = appSettings;
            this.site = site;
            _unityContainer = unityContainer;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel; 
            if (model != null)
            {
                model.Site = new SiteViewModel(site, _unityContainer);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}