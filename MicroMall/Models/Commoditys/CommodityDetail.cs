using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Commoditys
{
    public class CommodityDetail : CommodityModel
    {
        public CommodityDetail(Commodity item, string url, List<SpecificationAndSpecificationDetail> Specifications, ListReviewView listReview)
            : base(item, url)
        {
            if(item!=null)
            {
              //commodityId = item.commodityId;
              //commodityName = item.commodityName;
              if(!string.IsNullOrWhiteSpace(item.images))
              {
                  ListimageUrl = new List<string>();
                  string[] sp = item.images.Split(',');
                  if (sp.Count() >= 4)
                  {
                      for (int i = 3; i < sp.Count(); i++)
                          ListimageUrl.Add(url + "/" + sp[i]);
                  }
                  //for (int i = 0; i < sp.Count();i++ )
                  //    ListimageUrl.Add(url + "/CommodityImages/" + sp[i]);
              }
              specifications = Specifications;
              sellQuantity = item.sellQuantity;
              //commodityPrice = item.commodityPrice;
              commodityInventory = item.commodityInventory;
              commodityFreight = item.commodityFreight;
              commodityRemark = item.commodityRemark;
              commodityDetails = item.commodityDetails.Replace("/MicroMalls/CommodityImages", url);
              ListReview = listReview;
            }
        }
        public decimal commodityInventory { get; set; }

        public decimal commodityFreight { get; set; }

        public string commodityRemark { get; set; }

        public string commodityDetails { get; set; }

        public List<string> ListimageUrl { get; set; }

        public List<SpecificationAndSpecificationDetail> specifications { get; set; }

        public ListReviewView ListReview { get; set; }
    }
}