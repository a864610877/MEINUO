using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;

namespace Ecard.Mvc.Models.Shoping
{
    public class ShopingModelBase : ViewModelBase
    {
      private Commodity _innerObject;

      public ShopingModelBase() 
        {
            _innerObject = new Commodity();
        }
        protected void SetInnerObject(Commodity comm)
        {
            _innerObject = comm;
        }

        [NoRender]
        public Commodity InnerObject
        {
            get { return _innerObject; }
        }
        [NoRender]
        public int commodityId
        {
            get { return InnerObject.commodityId; }
            set { InnerObject.commodityId = value; }
        }
        public int commodityCategoryId
        {
            get { return InnerObject.commodityCategoryId; }
            set { InnerObject.commodityCategoryId = value; }
        }
        public string commodityName
        {
            get { return InnerObject.commodityName; }
            set { InnerObject.commodityName = value; }
        }
        public string commdityKeyword
        {
            get { return InnerObject.commdityKeyword; }
            set { InnerObject.commdityKeyword = value; }
        }
        public string commodityRemark
        {
            get { return InnerObject.commodityRemark; }
            set { InnerObject.commodityRemark = value; }
        }

        public decimal commodityPrice
        {
            get { return InnerObject.commodityPrice; }
            set { InnerObject.commodityPrice = value; }
        }
        public int commodityInventory
        {
            get { return InnerObject.commodityInventory; }
            set { InnerObject.commodityInventory = value; }
        }
        public int commodityRank
        {
            get { return InnerObject.commodityRank; }
            set { InnerObject.commodityRank = value; }
        }
        public string commodityNo
        {
            get { return InnerObject.commodityNo; }
            set { InnerObject.commodityNo = value; }
        }

        public decimal commodityFreight
        {
            get { return InnerObject.commodityFreight; }
            set { InnerObject.commodityFreight = value; }
        }
        public string images
        {
            get { return InnerObject.images; }
            set { InnerObject.images = value; }
        }
        public string commodityDetails
        {
            get { return InnerObject.commodityDetails; }
            set { InnerObject.commodityDetails = value; }
        }
        public string specificationId
        {
            get { return InnerObject.specificationId; }
            set { InnerObject.specificationId = value; }
        }

        public bool IsPinkage
        {
            get { return InnerObject.IsPinkage; }
            set { InnerObject.IsPinkage = value; }
        }

        public decimal commodityJifen
        {
            get { return InnerObject.commodityJifen; }
            set { InnerObject.commodityJifen = value; }
        }

    }
}