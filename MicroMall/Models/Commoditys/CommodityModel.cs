using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Commoditys
{
    public class CommodityModel
    {
        public CommodityModel() { }
        public CommodityModel(Commodity item, string url)
        {
            if(item!=null)
            {
              commodityId = item.commodityId;
              commodityName = item.commodityName;
              if(!string.IsNullOrWhiteSpace(item.images))
              {
                  string[] sp = item.images.Split(',');
                    if(sp.Count()>0)
                      imageUrl=(url + "/" + sp[0]);
              }
              //specifications = Specifications;
              sellQuantity = item.sellQuantity;
              commodityPrice = item.commodityPrice;
              commodityInventory = item.commodityInventory;
              //commodityFreight = item.commodityFreight;
              //commodityRemark = item.commodityRemark;
              //commodityDetails = item.commodityDetails;
            }
        }

        public int commodityId { get; set; }

        public string commodityName { get; set; }

        //public string commodityNo { get; set; }

        public string imageUrl { get; set; }

        public int sellQuantity { get; set; }

        public decimal commodityPrice { get; set; }

        public int commodityInventory { get; set; }

       
    }
}