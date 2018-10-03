using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SecondKillCommodityss
{
    public class SecondKillCommodityModelBase: ViewModelBase
    {
        private SecondKillCommoditys _innerObject;

        public SecondKillCommodityModelBase()
        {
            _innerObject = new SecondKillCommoditys();
        }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string commodityNo { get; set; }
        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int num { get; set; }

    }
}
