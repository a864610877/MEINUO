using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.CommodityCategory
{
    public class EditCommodityCategory : EcardModelListRequest<Ecard.Models.fz_CommodityCategorys>
    {
        public int commodityCategoryId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        [Dependency]
        [NoRender]
        public ICommodityCategorysService ICommodityCategorysService { get; set; }

        public void Ready(int id)
        {
            var item = ICommodityCategorysService.GetById(id);
            if (item != null)
            {
                this.commodityCategoryId = item.commodityCategoryId;
                this.Name = item.name;
            }
        }
    }
}
