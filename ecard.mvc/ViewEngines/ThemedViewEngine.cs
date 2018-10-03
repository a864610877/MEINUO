using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Moonlit.Web.Mvc.ViewEngines
{
    public class ThemedViewEngine : IViewEngine
    {
        private readonly IViewPageActivator _viewPageActivator;
        // format is ":ViewCacheEntry:{cacheType}:{prefix}:{name}:{appName}:{theme}:"
        private const string _cacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:";
        private const string _cacheKeyPrefix_Master = "Master";
        private const string _cacheKeyPrefix_Partial = "Partial";
        private const string _cacheKeyPrefix_View = "View";
        private static readonly string[] _emptyLocations = new string[0];

        internal Func<string, string> GetExtensionThunk = VirtualPathUtility.GetExtension;

        public string[] FileExtensions { get { return ViewFactory.FileExtensions; } }
        public string[] AppNameMasterLocationFormats { get; set; }

        public string[] AppNamePartialViewLocationFormats { get; set; }

        public string[] AppNameViewLocationFormats { get; set; }
        public string[] MasterLocationFormats { get; set; }

        public string[] PartialViewLocationFormats { get; set; }

        public IViewLocationCache ViewLocationCache { get; set; }

        public IViewFactory ViewFactory { get; set; }
        public string[] ViewLocationFormats { get; set; }

        public ThemedViewEngine(IViewPageActivator viewPageActivator)
        {
            _viewPageActivator = viewPageActivator;
            if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled)
            {
                ViewLocationCache = DefaultViewLocationCache.Null;
            }
            else
            {
                ViewLocationCache = new DefaultViewLocationCache();
            }

            // {1}appName , {2} theme
            AppNameViewLocationFormats = new[] {
                "~/Views/{1}/{2}/{0}.cshtml",
                "~/Views/{1}/{2}/Shared/{0}.cshtml",
                "~/Views/{1}/Default/{0}.cshtml",
                "~/Views/{1}/Default/Shared/{0}.cshtml",

                "~/Views/default/{0}.cshtml",
            };
            AppNameMasterLocationFormats = new[] {
                "~/Views/{1}/{2}/{0}.cshtml",
                "~/Views/{1}/{2}/Shared/{0}.cshtml",
                "~/Views/{1}/Default/{0}.cshtml",
                "~/Views/{1}/Default/Shared/{0}.cshtml",

                "~/Views/default/Shared/{0}.cshtml", 
            };
            AppNamePartialViewLocationFormats = new[] {
                "~/Views/{1}/{2}/{0}.cshtml",
                "~/Views/{1}/{2}/Shared/{0}.cshtml",
                "~/Views/{1}/Default/{0}.cshtml",
                "~/Views/{1}/Default/Shared/{0}.cshtml",

                "~/Views/default/Shared/{0}.cshtml",
            }; 
        }

        private string CreateCacheKey(string prefix, string name, string appName, string themeName)
        {
            return String.Format(CultureInfo.InvariantCulture, _cacheKeyFormat,
                                 GetType().AssemblyQualifiedName, prefix, name, appName, themeName);
        }

        protected IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return this.ViewFactory.CreatePartialView(controllerContext, partialPath, _viewPageActivator);
        }

        protected IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return this.ViewFactory.CreateView(controllerContext, viewPath, masterPath, _viewPageActivator);
        }

        protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return BuildManager.GetObjectFactory(virtualPath, false) != null;
        }

        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentNullException("partialViewName");
            }

            string[] searched;
            string themeName = "default";
            string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AppNamePartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, themeName, _cacheKeyPrefix_Partial, useCache, out searched);

            if (String.IsNullOrEmpty(partialPath))
            {
                return new ViewEngineResult(searched);
            }

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }

        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (String.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            string themeName = "default";
            string viewPath = GetPath(controllerContext, ViewLocationFormats, AppNameViewLocationFormats, "ViewLocationFormats", viewName, themeName, _cacheKeyPrefix_View, useCache, out viewLocationsSearched);
            string masterPath = GetPath(controllerContext, MasterLocationFormats, AppNameMasterLocationFormats, "MasterLocationFormats", masterName, themeName, _cacheKeyPrefix_Master, useCache, out masterLocationsSearched);

            if (String.IsNullOrEmpty(viewPath) || (String.IsNullOrEmpty(masterPath) && !String.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string themeName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = _emptyLocations;

            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }
            object appName;
            bool usingAreas = controllerContext.RouteData.Values.TryGetValue("appName", out appName);
            List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);

            if (viewLocations.Count == 0)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture,
                                                                  "didn't find view {0} property", locationsPropertyName));
            }

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(cacheKeyPrefix, name, (nameRepresentsPath) ? String.Empty : themeName, themeName);

            if (useCache)
            {
                return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
            }

            return (nameRepresentsPath) ?
                GetPathFromSpecificName(controllerContext, name, cacheKey, ref searchedLocations) :
                GetPathFromGeneralName(controllerContext, viewLocations, name, themeName, (appName ?? "").ToString(), cacheKey, ref searchedLocations);
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, List<ViewLocation> locations, string name, string themeName, string appName, string cacheKey, ref string[] searchedLocations)
        {
            string result = String.Empty;
            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                ViewLocation location = locations[i];
                string virtualPath = location.Format(name, themeName, appName);

                if (FileExists(controllerContext, virtualPath))
                {
                    searchedLocations = _emptyLocations;
                    result = virtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string result = name;

            if (!(FilePathIsSupported(name) && FileExists(controllerContext, name)))
            {
                result = String.Empty;
                searchedLocations = new[] { name };
            }

            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            return result;
        }

        private bool FilePathIsSupported(string virtualPath)
        {
            if (FileExtensions == null)
            {
                // legacy behavior for custom ViewEngine that might not set the FileExtensions property
                return true;
            }

            // get rid of the '.' because the FileExtensions property expects extensions withouth a dot.
            string extension = GetExtensionThunk(virtualPath).TrimStart('.');
            return FileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        private static List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            List<ViewLocation> allLocations = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                foreach (string areaViewLocationFormat in areaViewLocationFormats)
                {
                    allLocations.Add(new AreaAwareViewLocation(areaViewLocationFormat));
                }
            }

            if (viewLocationFormats != null)
            {
                foreach (string viewLocationFormat in viewLocationFormats)
                {
                    allLocations.Add(new ViewLocation(viewLocationFormat));
                }
            }

            return allLocations;
        }

        private static bool IsSpecificPath(string name)
        {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        public virtual void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        private class ViewLocation
        {

            protected string _virtualPathFormatString;

            public ViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string themeName, string appName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, appName);
            }

        }

        private class AreaAwareViewLocation : ViewLocation
        {

            public AreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string themeName, string appName)
            {
                return String.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, appName, themeName);
            }

        }
    }
}