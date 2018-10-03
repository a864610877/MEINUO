using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc;
using Ecard.Requests;
using Ecard.Services;
using MicroMall.Models.Commoditys;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Homes
{
    public class LoadIndex : LayoutModel
    {
        public LoadIndex()
        {
           
        }
        private int _nextPage;
        public int NextPage { get { return _nextPage; } }

        private List<CommodityModel> _listCommodity;
        public List<CommodityModel> ListCommodity { get { return _listCommodity; } }
         [Dependency]
        public ICommodityService CommodityService { get; set; }
         [Dependency]
         public IAdService AdService { get; set; }

         private List<AdModel> _listAd;
         public List<AdModel> ListAd { get { return _listAd; } }
        
         //public  

         public dataJson GetCommodity(int IndexPage, string content)
        {
            try
            {
                var request = new CommodityRequest();
                request.commodityName = content;
                request.commdityKeyword = content;
                request.PageIndex = IndexPage;
                request.commodityState = CommodityStates.putaway;
                var tables = CommodityService.Query(request);
                if (tables.TotalCount > 0)
                {
                    int TotalPage = Math.Max((tables.TotalCount + request.PageSize - 1) / request.PageSize, 1);
                    if (IndexPage == TotalPage)
                    {
                        _nextPage = 0;
                    }
                    else if (IndexPage < TotalPage)
                    {
                        _nextPage = IndexPage+1;
                        
                    }
                    var list = tables.ModelList.Select(x => new CommodityModel(x, ImageUrl)).ToList();
                    _listCommodity = list;
                    return new dataJson() {nextPage=_nextPage,events=list };
                }
                return new dataJson();
            }
            catch(Exception ex)
            {
                logService.Insert(ex);
                return null;
            }
        }

        public List<AdModel> GetAd()
        {
            var query = AdService.MicroMallQuery();
            return query.Select(x => new AdModel(x,ImageUrl)).ToList();
        }

        public void Ready()
        {
           Load();
           GetCommodity(1, null);
           _listAd = GetAd();
        }

        //public List<CommodityModel> Search(string )
        
    }

}