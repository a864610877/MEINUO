using System;
using System.IO;
using System.Web.Mvc;

namespace Moonlit.Web.Mvc.ViewEngines
{
    public class RazorViewFactory : IViewFactory
    {
        IView IViewFactory.CreatePartialView(ControllerContext controllerContext, string partialPath, IViewPageActivator viewPageActivator)
        {
            var view = new RazorView(controllerContext, partialPath,
                                    layoutPath: null, runViewStartPages: false, viewStartFileExtensions: FileExtensions, viewPageActivator: viewPageActivator);

            return view;
        }

        IView IViewFactory.CreateView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator)
        {
            var view = new RazorView(controllerContext, viewPath,
                                     layoutPath: masterPath, runViewStartPages: true, viewStartFileExtensions: FileExtensions, viewPageActivator: viewPageActivator);
            return view;
        }

        public string[] FileExtensions
        {
            get { return new[] { "bshtml", "cshtml" }; }
        }
    }
}
     