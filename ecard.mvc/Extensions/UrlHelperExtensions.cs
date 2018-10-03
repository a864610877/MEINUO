//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Ecard.Mvc.Models;
using Ecard.Mvc.ViewModels;
using Oxite.Model;
using Oxite.Routing;
using System.Web;

namespace Ecard.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string Oxite(this UrlHelper urlHelper)
        {
            return "http://oxite.net";
        }
        public static string UserPhoto(this UrlHelper urlHelper, string url)
        {
            return urlHelper.Content(string.Format(Paths.UserPhotoFormat, url));
        }

        public static string AbsolutePath(this UrlHelper urlHelper, string relativeUrl)
        {
            Uri url = urlHelper.RequestContext.HttpContext.Request.Url;
            UriBuilder uriBuilder = new UriBuilder(url.Scheme, url.Host, url.Port) { Path = relativeUrl };
            //string appPath = urlHelper.RequestContext.HttpContext.Request.ApplicationPath;

            //if (!uriBuilder.Path.StartsWith(appPath))
            //    uriBuilder.Path = appPath + uriBuilder.Path;

            return uriBuilder.Uri.ToString();
        }

        public static string AppPath(this UrlHelper urlHelper, string relativePath)
        {
            if (relativePath == null) return null;

            return VirtualPathUtility.ToAbsolute(relativePath, urlHelper.RequestContext.HttpContext.Request.ApplicationPath);
        }

        public static string CssPath(this UrlHelper urlHelper, EcardModel model)
        {
            return CssPath(urlHelper, "", model);
        }

        public static string CssPath(this UrlHelper urlHelper, string relativePath, EcardModel model)
        {
            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/"))
            {
                relativePath = "/" + relativePath;
            }

            return string.Format(urlHelper.AppPath(model.Site.CssPath), model.SkinName) + relativePath;
        }

        public static string ScriptPath(this UrlHelper urlHelper, EcardModel model)
        {
            return ScriptPath(urlHelper, "", model);
        }

        public static string ScriptPath(this UrlHelper urlHelper, string relativePath, EcardModel model)
        {
            if (!string.IsNullOrEmpty(relativePath) && !relativePath.StartsWith("/"))
            {
                relativePath = "/" + relativePath;
            }

            return string.Format(urlHelper.AppPath(model.Site.ScriptsPath), model.SkinName) + relativePath;
        } 

        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Home");
        }
 
        public static string Page(this UrlHelper urlHelper, string pagePath)
        {
            return urlHelper.RouteUrl("Page", new { pagePath });
        }
 

        public static string AddPage(this UrlHelper urlHelper)
        {
            return urlHelper.AddPage(urlHelper.RequestContext.RouteData.Values["pagePath"] as string);
        }

        public static string AddPage(this UrlHelper urlHelper, string pagePath)
        {
            if (string.IsNullOrEmpty(pagePath))
                pagePath = urlHelper.RequestContext.RouteData.Values["pagePath"] as string;

            if (!string.IsNullOrEmpty(pagePath))
            {
                if (pagePath.EndsWith("/" + PageMode.Add))
                    pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Add).Length);
                else if (pagePath.EndsWith("/" + PageMode.Edit))
                    pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Edit).Length);
                else if (pagePath.EndsWith("/" + PageMode.Remove))
                    pagePath = pagePath.Substring(0, pagePath.Length - ("/" + PageMode.Remove).Length);
            }

            return !string.IsNullOrEmpty(pagePath)
                ? urlHelper.RouteUrl("AddPageToPage", new { pagePath = pagePath + "/Add" })
                : urlHelper.RouteUrl("AddPageToSite");
        }
         
        public static string Search(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("PostsBySearch");
        }

        public static string Search(this UrlHelper urlHelper, string term)
        {
            return urlHelper.RouteUrl("PostsBySearch", new { term });
        }

        public static string Search(this UrlHelper urlHelper, string dataFormat, string term)
        {
            return urlHelper.RouteUrl("PostsBySearch", new { dataFormat = dataFormat, term = term });
        }

        public static string OpenSearch(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("OpenSearch");
        }

        public static string OpenSearchOSDX(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("OpenSearchOSDX");
        }
          
        public static string Rsd(this UrlHelper urlHelper, string areaName)
        {
            if (!string.IsNullOrEmpty(areaName))
                return urlHelper.RouteUrl("AreaRsd", new { areaName });

            return urlHelper.RouteUrl("Rsd");
        }
  

        public static string SiteMapIndex(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("SiteMapIndex");
        }

        public static string SiteMap(this UrlHelper urlHelper, int year, int month)
        {
            return urlHelper.RouteUrl("SiteMap", new { year, month });
        }

        public static string MetaWeblogApi(this UrlHelper urlHelper)
        {
            return urlHelper.AppPath("~/MetaWeblog.svc");
        }

        public static string SignIn(this UrlHelper urlHelper, string returnUrl)
        {
            return urlHelper.RouteUrl("SignIn", new { ReturnUrl = returnUrl });
        }

        public static string SignOut(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("SignOut");
        }

        public static string ComputeHash(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ComputeHash");
        }

        public static string Site(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Default");
        }

        public static string ManageSite(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageSite");
        }

        public static string ManageAreas(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageAreas");
        }

        public static string ManageUsers(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("ManageUsers");
        }

        public static string UserChangePassword(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("UserChangePassword");
        }

        public static string Plugins(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Plugins");
        }

        public static string AreaFind(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("AreaFind");
        }

        public static string AreaAdd(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("AreaAdd");
        } 
        public static string Admin(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Admin");
        }
 
        public static bool IsHome(this UrlHelper urlHelper)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;
            string action = urlHelper.RequestContext.RouteData.Values["action"] as string;

            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Post", true) == 0 &&
                !string.IsNullOrEmpty(action) &&
                string.Compare(action, "List", true) == 0;
        }

        public static bool IsPagePath(this UrlHelper urlHelper, string pagePath)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;
            string pagePathValue = urlHelper.RequestContext.RouteData.Values["pagePath"] as string;

            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Page", true) == 0 &&
                !string.IsNullOrEmpty(pagePath) &&
                string.Compare(pagePathValue, pagePath, true) == 0;
        }

        public static bool IsAdmin(this UrlHelper urlHelper)
        {
            string controller = urlHelper.RequestContext.RouteData.Values["controller"] as string;

            //TODO: (erikpo) Once the admin is refactored, change this to look at the url to determine if we're in the admin or not
            return
                !string.IsNullOrEmpty(controller) &&
                string.Compare(controller, "Admin", true) == 0;
        }
    }
}
