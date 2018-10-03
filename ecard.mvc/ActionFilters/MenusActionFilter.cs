using System.Web.Mvc;
using Ecard.Mvc.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Oxite.Mvc.Extensions;

namespace Ecard.Mvc.ActionFilters
{
    public class MenusActionFilter : IActionFilter
    {
        private readonly IMenuService _menuService;
        private readonly SecurityHelper _securityHelper;

        public MenusActionFilter(IMenuService menuService, SecurityHelper securityHelper)
        {
            _menuService = menuService;
            _securityHelper = securityHelper;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel;

            if (model != null)
            {
                var user = _securityHelper.GetCurrentUser();
                if (user != null)
                    model.Menus = _menuService.GetMenus(user.CurrentUser);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // refresh cache
            var user = _securityHelper.GetCurrentUser();
            
        }
    }
}