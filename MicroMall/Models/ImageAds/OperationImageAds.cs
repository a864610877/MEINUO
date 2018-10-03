using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ImageAds
{
    public class OperationImageAds
    {
        string url = System.Configuration.ConfigurationManager.AppSettings["adminUrl"];

        [Dependency]
        public IImageAdsService ImageAdsService { get; set; }


    }
}