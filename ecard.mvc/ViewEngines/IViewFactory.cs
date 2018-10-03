using System.Web.Mvc;

namespace Moonlit.Web.Mvc.ViewEngines
{
    public interface IViewFactory
    {
        IView CreatePartialView(ControllerContext controllerContext, string partialPath, IViewPageActivator viewPageActivator);
        IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator);
        string[] FileExtensions { get; }
    }
}