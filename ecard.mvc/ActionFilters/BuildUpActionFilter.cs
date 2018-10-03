using System;
using System.Web.Mvc;
using Ecard.Mvc.ViewModels;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.ActionFilters
{
    public class BuildUpActionFilter : IActionFilter
    {
        private readonly IUnityContainer _unityContainer;

        public BuildUpActionFilter(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            EcardModel model = filterContext.Controller.ViewData.Model as EcardModel;

            if (model != null)
            {
                _unityContainer.BuildUp(model.GetType(), model);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var parameter in filterContext.ActionParameters)
            {
                if (parameter.Value != null && Type.GetTypeCode(parameter.Value.GetType()) == TypeCode.Object)
                    _unityContainer.BuildUp(parameter.Value.GetType(), parameter.Value);
            }
            
        }
    }
}