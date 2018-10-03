using System.Web.Mvc;
using Ecard.Models;
using Ecard.Models.PageContainers;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Moonlit;
using Oxite.Mvc.Extensions;

namespace Ecard.Mvc.ActionFilters
{
    public class ContainerActionFilter : IActionFilter
    {
        private readonly IMenuService _menuService;
        private readonly SecurityHelper _securityHelper;
        private readonly I18NManager _i18NManager;
        private readonly IControllerFinder _controllerFinder;

        public ContainerActionFilter(IMenuService menuService, SecurityHelper securityHelper, I18NManager i18NManager, IControllerFinder controllerFinder)
        {
            _menuService = menuService;
            _securityHelper = securityHelper;
            _i18NManager = i18NManager;
            _controllerFinder = controllerFinder;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel;

            if (model != null && model.Container == null)
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();

                var controllerType = _controllerFinder.FindController(controller);
                if (controllerType == null)
                    return;

                var controllerTypeDesc = ViewModelDescriptor.GetTypeDescriptor(controllerType);

                PageContainer c = new PageContainer();
                c.Description = controllerTypeDesc.Description;
                c.DisplayName = controllerTypeDesc.LongName;
                c.Name = controllerTypeDesc.Name;

                model.Container = c;
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}