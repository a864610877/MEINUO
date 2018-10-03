using Ecard.Models;
using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SecondKillCommodityss
{
    public class AddSecondKillCommodity : SecondKillCommodityModelBase, IValidator
    {
        public AddSecondKillCommodity()
        {
           
        }
        [NoRender, Dependency]
        public ISecondKillCommoditysService SecondKillCommoditysService { get; set; }
        [NoRender, Dependency]
        public ICommodityService CommodityService { get; set; }
        public IEnumerable<ValidationError> Validate()
        {
            yield break;
        }

        public IMessageProvider Create()
        {
            if (string.IsNullOrWhiteSpace(commodityNo))
            {
                AddError(1,"请输入商品编码");
                return this;
            }
            var commdity = CommodityService.GetBycommodityNo(commodityNo);
            if (commdity == null)
            {
                AddError(1, "商品编码不存在");
                return this;
            }
            var model = new SecondKillCommoditys();
            model.commodityId = commdity.commodityId;
            model.createTime = DateTime.Now;
            model.num = num;
            model.surplusNum = num - model.payNum;
            model.price = price;
            model.commodityNo = commodityNo;
            SecondKillCommoditysService.Insert(model);
            AddMessage("success");
            return this;
        } 
    }
}
