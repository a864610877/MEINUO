using Ecard.Models;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Shoping
{
    public class CreateShoping : Commodity
    {
        [Dependency,NoRender]
        public ICommodityService commodityService{ get; set; }
        [Dependency, NoRender]
        public TransactionHelper TransactionHelper { get; set; }
        [Dependency, NoRender]
        public ISpecificationService specificationService { get; set; }

        public string Create() 
        {
          Commodity commodity = new Commodity();
          commodity.commdityKeyword = this.commdityKeyword;
          commodity.images = this.images;
          commodity.commodityDetails = this.commodityDetails;
          commodity.commodityFreight = this.commodityFreight;
          commodity.commodityInventory = this.commodityInventory;
          commodity.commodityName = this.commodityName;
          commodity.commodityNo = this.commodityNo;
          commodity.commodityPrice = this.commodityPrice;
          commodity.commodityRank = this.commodityRank;
          commodity.commodityRemark = this.commodityRemark;
          commodity.commodityState = CommodityStates.soldOut;
          commodity.submitTime = DateTime.Now;
          commodity.specificationId = this.specificationId;
          commodity.IsPinkage = this.IsPinkage;
          commodity.commodityCategoryId = this.commodityCategoryId;
            commodity.commodityJifen = this.commodityJifen;
          commodityService.Insert(commodity);
          return "新增商品成功！";
        }
    }
}
