//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ecard.Models;
using Oxite.Infrastructure;
using Oxite.Routing;

namespace Ecard.Routing
{
    /// <summary>
    /// 注册和管理所有 url route
    /// </summary>
    public class EcardRoutes : IRegisterRoutes
    {
        private readonly RouteCollection _routes;
        private readonly AppSettingsHelper _appSettings;
        private readonly Site _site;

        public EcardRoutes(RouteCollection routes, AppSettingsHelper appSettings, Site site)
        {
            _routes = routes;
            _appSettings = appSettings;
            _site = site;
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public virtual void RegisterRoutes()
        {
            _routes.Clear();


            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);

            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterRoutes(_routes, _appSettings.GetStringArray("ControllerNamespaces", ",", null));
        }

        protected virtual void RegisterRoutes(RouteCollection existingRoutes, string[] controllerNamespaces)
        {
            RegisterHomeRoutes(controllerNamespaces);

            //RegisterSearchRoutes(controllerNamespaces);

            //RegisterUserRoutes(controllerNamespaces);

            //RegisterSEORoutes(controllerNamespaces);
            RegisterDefaultRoutes(controllerNamespaces);
        }

        private void RegisterDefaultRoutes(string[] controllerNamespaces)
        {  
            AddRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { }
                );
        }

        protected virtual void RegisterHomeRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "SignIn",
                "SignIn",
                new { controller = "User", action = "LogOn" },
                new { }
                ); 
            AddRoute(
                "SignOut",
                "SignOut",
                new { controller = "User", action = "LogOff" },
                new { }
                ); 
            AddRoute(
                "EditProfile",
                "EditProfile",
                new { controller = "User", action = "EditProfile" },
                new { }
                ); 
            AddRoute(
                "Code",
                "Code",
                new { controller = "Utility", action = "Code" },
                new { }
                ); 
            AddRoute(
                "Recovery",
                "Recovery/{token}",
                new { controller = "User", action = "Recovery" },
                new { }
                ); 
        }
        protected virtual void RegisterUserRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "User",
                "User",
                new { controller = "User", action = "UserInfo" },
                null
                );

            AddRoute(
                "SignOut",
                "SignOut",
                new { controller = "User", action = "SignOut" },
                null
                );

            AddRoute(
                "UserChangePassword",
                "Admin/ChangePassword",
                new { controller = "User", action = "ChangePassword", validateAntiForgeryToken = true },
                null
                );
        }
        protected virtual void RegisterSearchRoutes(string[] controllerNamespaces)
        {
            //if (_site.IncludeOpenSearch)
            //{
            //    AddRoute(
            //        "OpenSearch",
            //        "OpenSearch.xml",
            //        new { controller = "Utility", action = "OpenSearch" },
            //        null
            //        );

            //    AddRoute(
            //        "OpenSearchOSDX",
            //        "OpenSearch.osdx",
            //        new { controller = "Utility", action = "OpenSearchOSDX" },
            //        null
            //        );
            //} 
        }
        protected virtual void RegisterSEORoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "RobotsTxt",
                "robots.txt",
                new { controller = "Utility", action = "RobotsTxt" },
                null
                );

            AddRoute(
                "SiteMapIndex",
                "SiteMap",
                new { controller = "Utility", action = "SiteMapIndex" },
                null
                );

            AddRoute(
                "SiteMap",
                "SiteMap/{year}/{month}",
                new { controller = "Utility", action = "SiteMap" },
                new
                {
                    year = new IsInt(DateTime.MinValue.Year, DateTime.MaxValue.Year),
                    month = new IsInt(DateTime.MinValue.Month, DateTime.MaxValue.Month)
                }
                );
        }

        protected virtual void AddRoute(string name, string url, object defaults, object constraints)
        { 
            //if (_site.RouteUrlPrefix == null)
            //{
            //    url = "oxite.aspx/" + url;
            //}
            //else if (_site.RouteUrlPrefix != "")
            //{
            //    url = _site.RouteUrlPrefix + "/" + url;
            //}
            _routes.MapRoute(
                name, // Route name
                url, // URL with parameters
                defaults,
                constraints
            );
        } 
    }
}
 