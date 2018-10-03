//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.ViewModels
{
    /// <summary>
    /// 站点相关信息
    /// </summary>
    public class SiteViewModel
    {
        private readonly IUnityContainer _unityContainer;

        public SiteViewModel(Site currentSite, IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            //ID = currentSite.MixCode;
            DisplayName = currentSite.DisplayName ?? "Ecard Site";
            Description = currentSite.Description;
            ScriptsPath = "~/Skins/{0}/Scripts";
            CssPath = "~/Skins/{0}/Styles";
            //IncludeOpenSearch = currentSite.IncludeOpenSearch;
            SkinDefault = "Default";
            FavIconUrl = currentSite.FavIconUrl ?? "~/Content/icons/flame.ico";
            //CommentingDisabled = currentSite.CommentingDisabled;
            Host = HttpContext.Current.Request.Url.Host;
            Version = currentSite.Version;
            //PosType = currentSite.PosType;
            //AuthType = currentSite.AuthType;
            //PasswordType = currentSite.PasswordType;
            //PrinterType = currentSite.PrinterType;
            CopyRight = currentSite.CopyRight;
            //AccountToken = currentSite.AccountToken;
            //AccountNameLength = currentSite.AccountNameLength; 

            ShopNameLength = 15;
            PosNameLength = 8;

            HowToDeals = new List<string>();
            //if (!string.IsNullOrWhiteSpace(currentSite.HowToDeals))
            //{
            //    var howToDeals = currentSite.HowToDeals.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //    HowToDeals = howToDeals.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
            //}
        }

        internal string ID { get; private set; }
        public string AccountToken { get; private set; }
        public string DisplayName { get; private set; }
        public string Host { get; private set; }
        public string Description { get; private set; }
        public string ScriptsPath { get; private set; }
        public string CopyRight { get; private set; }
        public string PosType { get; private set; }
        public string PrinterType { get; private set; }
        public string PasswordType { get; set; }
        public string AuthType { get; private set; }
        public string CssPath { get; private set; }
        public bool IncludeOpenSearch { get; set; }
        public string SkinDefault { get; set; }
        public string FavIconUrl { get; set; }
        public bool CommentingDisabled { get; set; }
        public int AccountNameLength { get; set; }
        public int ShopNameLength { get; set; }
        public int PosNameLength { get; set; }
        public List<string> HowToDeals { get; set; }

        public decimal Version { get; private set; }

        public static string[] GetPosTypes()
        {
            return new[] { "none", "YLE300", "ISO14443A","W-I" };
        }
        public static string[] GetPrinterTypes()
        {
            return new[] { "alert", "default", "navandprint", "nav" };
        }

        public IEnumerable<IdNames> GetPrinters()
        {
            foreach (var printerType in GetPrinterTypes())
            {
                var printer = _unityContainer.Resolve<IPrinterService>(printerType);
                yield return new IdNames { Id = printerType, Name = printer.Name };
            }
        }
    }
    public class IdNames
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
