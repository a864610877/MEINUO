using Ecard.Services;
using Ecard.Validation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SecondKillCommodityss
{
    public class EditSecondKillCommodity : SecondKillCommodityModelBase, IValidator
    {
        [NoRender, Dependency]
        public ISecondKillCommoditysService SecondKillCommoditysService { get; set; }
        [NoRender, Dependency]
        public ICommodityService CommodityService { get; set; }
        public IEnumerable<ValidationError> Validate()
        {
            yield break;
        }
        [Hidden]
        public int Id { get; set; }

        public void Ready(int id)
        {
            var item = SecondKillCommoditysService.GetById(id);
            if (item != null)
            {
                this.commodityNo = item.commodityNo;
                this.Id = item.id;
                this.num = item.num;
                this.price = item.price;
            }
        }

        public IMessageProvider Save()
        {
            if (string.IsNullOrWhiteSpace(commodityNo))
            {
                AddError(1, "请输入商品编码");
                return this;
            }
            var commdity = CommodityService.GetBycommodityNo(commodityNo);
            if (commdity == null)
            {
                AddError(1, "商品编码不存在");
                return this;
            }
            var model = SecondKillCommoditysService.GetById(Id);
            if (model != null)
            {
                model.commodityId = commdity.commodityId;
                model.num = num;
                model.price = price;
                model.surplusNum = num - model.payNum;
                model.commodityNo = commodityNo;
                SecondKillCommoditysService.Update(model);
            }
            AddMessage("success");
            return this;
        } 
    }
}
