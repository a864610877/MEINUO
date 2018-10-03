using MicroMall.Models.ImageAds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class JuMeiMallListModel
    {
        public JuMeiMallListModel()
        {
            JuMeiMallList = new List<JuMeiMallModel>();
            AdsList = new List<ImageAdsModel>();
            CateMallList = new List<CateMallModel>();
            JuMeiMallExList = new List<JuMeiMallModelExpress>();
        }
        public List<JuMeiMallModel> JuMeiMallList { get; set; }
        public List<JuMeiMallModelExpress> JuMeiMallExList { get; set; }

        public List<ImageAdsModel> AdsList { get; set; }
        public List<CateMallModel> CateMallList { get; set; }


        public int totalCount { get; set; }

        public int pageIndex { get; set; }

        public string categoryName { get; set; }


    }
}