using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Shoping
{
    public class EditShoping : ShopingModelBase
    {

        [Dependency]
        [NoRender]
        public ICommodityService commodityService { get; set; }
        [Dependency]
        [NoRender]
        public ISpecificationService specificationService { get; set; }
        [Dependency]
        [NoRender]
        public ISpecificationDetailService specificationDetailService { get; set; }

        public List<SpecificationAndSpecificationDetail> SpAndDetail;

        public void Ready(int id)
        {
            var comm = commodityService.GetById(id);
            if (comm != null)
            {
                if (comm.specificationId != null)
                {
                    SpAndDetail = new List<SpecificationAndSpecificationDetail>();
                    var sp = comm.specificationId.Split(',');
                    for (int i = 0; i < sp.Length; i++)
                    {
                        var spAndDetail = specificationService.GetSpecificationAndSpecificationDetailById(Convert.ToInt32(sp[i]));
                        SpAndDetail.Add(spAndDetail);
                    }
                }

                SetInnerObject(comm);
            }
        }

        public ResultMsg Save()
        {
            var comm = commodityService.GetById(this.commodityId);
             ResultMsg result = new ResultMsg();
            if (comm!=null)
            {
                comm.commdityKeyword = commdityKeyword;
                comm.commodityDetails = commodityDetails;
                comm.commodityFreight = commodityFreight;
                comm.commodityInventory = commodityInventory;
                comm.commodityName = commodityName;
                comm.specificationId = specificationId;
                comm.commodityPrice = commodityPrice;
                comm.commodityRank = commodityRank;
                comm.commodityRemark = commodityRemark;
                comm.images = images;
                comm.IsPinkage = IsPinkage;
                comm.commodityCategoryId = commodityCategoryId;
                comm.commodityJifen = commodityJifen;
                commodityService.Update(comm);
                result.Code=1;
                result.CodeText="修改成功!";
                return result;
            }
            else
            {
                result.Code = 2;
                result.CodeText = "修改失败!";
                return result;
            }
           
        }
    }
}
