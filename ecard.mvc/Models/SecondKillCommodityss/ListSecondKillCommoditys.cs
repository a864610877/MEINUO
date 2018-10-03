using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SecondKillCommodityss
{
    public class ListSecondKillCommoditys
    {
        private readonly SecondKillCommoditysss _innerObject;
        [NoRender]
        public SecondKillCommoditysss InnerObject
        {
            get { return _innerObject; }
        }

        public ListSecondKillCommoditys()
        {
            _innerObject = new SecondKillCommoditysss();
        }

        public ListSecondKillCommoditys(SecondKillCommoditysss innerObject)
        {
            _innerObject = innerObject;
        }

        [NoRender]
        public int Id { get { return InnerObject.id; } }

        public string commodityNo { get { return InnerObject.commodityNo; } }

        public string commodityName { get { return InnerObject.commodityName; } }

        public int num { get { return InnerObject.num; } }

        public decimal price { get { return InnerObject.price; } }

        public int surplusNum { get { return InnerObject.surplusNum; } }

        public int payNum { get { return InnerObject.payNum; } }
        [NoRender]
        public string boor { get; set; }
    }
}
