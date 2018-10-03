using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models.Sites
{
    public class EditSite
    {
        [Dependency, NoRender]
        public IUnityContainer UnityContainer { get; set; }
        [Dependency, NoRender]
        public ISiteService SiteService { get; set; }
        [Dependency, NoRender]
        public ICacheService CacheService { get; set; }

        [Dependency, NoRender]
        public Site Site { get; set; }
        Site GetSite()
        {
            return Site ?? InnerDto;
        }
        [NoRender]
        public Site InnerDto { get; set; }

        public EditSite()
        {
            InnerDto = new Site();
        }

        //public string Name
        //{
        //    get { return GetSite().Name; }
        //    set { InnerDto.Name = value; }
        //}
        public string DisplayName
        {
            get { return GetSite().DisplayName; }
            set { InnerDto.DisplayName = value; }
        }
        public string CopyRight
        {
            get { return GetSite().CopyRight; }
            set { InnerDto.CopyRight = value; }
        } 
        public string Description
        {
            get { return GetSite().Description; }
            set { InnerDto.Description = value; }
        }

        public string imageUrl
        {
            get { return GetSite().imageUrl; }
            set { InnerDto.imageUrl = value; }
        }

        public string adminUrl
        {
            get { return GetSite().adminUrl; }
            set { InnerDto.adminUrl = value; }
        }

        public string Url
        {
            get { return GetSite().Url; }
            set { InnerDto.Url = value; }
        }

        //public decimal PointRatio
        //{
        //    get { return GetSite().PointRatio; }
        //    set { InnerDto.PointRatio = value; }
        //}

        //public decimal RebateRatio
        //{
        //    get { return GetSite().RebateRatio; }
        //    set { InnerDto.RebateRatio = value; }
        //}

        public decimal MinAmount
        {
            get { return GetSite().minAmount; }
            set { InnerDto.minAmount = value; }
        }
       
       
        public decimal OneRebate
        {
            get { return GetSite().OneRebate; }
            set { InnerDto.OneRebate = value; }
        }
        //public int OnePoint
        //{
        //    get { return GetSite().OnePoint; }
        //    set { InnerDto.OnePoint = value; }
        //}
        
        public decimal TwoRebate
        {
            get { return GetSite().TwoRebate; }
            set { InnerDto.TwoRebate = value; }
        }
        public int TwoPoint
        {
            get { return GetSite().TwoPoint; }
            set { InnerDto.TwoPoint = value; }
        }
        public int TwoPeople
        {
            get { return GetSite().TwoPeople; }
            set { InnerDto.TwoPeople = value; }
        }
        
        public decimal ThreeRebate
        {
            get { return GetSite().ThreeRebate; }
            set { InnerDto.ThreeRebate = value; }
        }
        public int ThreePoint
        {
            get { return GetSite().ThreePoint; }
            set { InnerDto.ThreePoint = value; }
        }
        public int ThreePeople
        {
            get { return GetSite().ThreePeople; }
            set { InnerDto.ThreePeople = value; }
        }

        

       
      

       
       
        public void Ready()
        {
            Site = SiteService.Query(null).FirstOrDefault();
        }

        private static string[] GetPosTypes()
        {
            return SiteViewModel.GetPosTypes();
        }

        private static string[] GetPasswordTypes()
        {
            return new[]
                       {
                           "none", 
            // delete for publish source code
                           "sle902r"
                       };
        }

        private static string[] GetPrinterTypes()
        {
            return SiteViewModel.GetPrinterTypes();
        }
        private static string[] GetAuthTypes()
        {
            return new[] { "password", "ikeyandpassword" };
        }

        public void Save()
        {
            Site.Name = InnerDto.Name;
            Site.DisplayName = InnerDto.DisplayName;
            Site.Description = InnerDto.Description;
            Site.PointRatio = InnerDto.PointRatio;
            Site.RebateRatio = InnerDto.RebateRatio;
            Site.imageUrl = InnerDto.imageUrl;
            Site.givePoint = InnerDto.givePoint;
            Site.OneRebate = InnerDto.OneRebate;
            Site.TwoRebate = InnerDto.TwoRebate;
            Site.TwoPeople = InnerDto.TwoPeople;
            Site.TwoPoint = InnerDto.TwoPoint;
            Site.ThreeRebate = InnerDto.ThreeRebate;
            Site.ThreePeople = InnerDto.ThreePeople;
            Site.ThreePoint = InnerDto.ThreePoint;
            Site.minAmount = InnerDto.minAmount;
            //Site.IsLimiteAmountApprove = InnerDto.IsLimiteAmountApprove;
            //Site.IsRechargingApprove = InnerDto.IsRechargingApprove;
           // Site.HowToDeals = InnerDto.HowToDeals;
            Site.CopyRight = InnerDto.CopyRight;
            Site.adminUrl = InnerDto.adminUrl;
            Site.Url = InnerDto.Url;
            SiteService.Update(Site);
            CacheService.Refresh(CacheKeys.SiteKey);
        }
    }
}