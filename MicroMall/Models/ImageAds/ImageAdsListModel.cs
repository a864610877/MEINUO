using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.ImageAds
{
    public class ImageAdsListModel
    {

        public ImageAdsListModel()
        {
            AdsList = new List<ImageAdsModel>();

        }
        public List<ImageAdsModel> AdsList { get; set; }

        //public int totalCount { get; set; }
    }
}