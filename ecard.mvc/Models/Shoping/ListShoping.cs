using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Shoping
{
    public class ListShoping
    {

        private readonly Commodity _innerObject;
        [NoRender]
        public Commodity InnerObject
        {
            get { return _innerObject; }
        }

        public ListShoping()
        {
            _innerObject = new Commodity();
        }

        public ListShoping(Commodity innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int commodityId { get { return InnerObject.commodityId; } }

        public string commodityNo { get { return InnerObject.commodityNo; } }

        public string commodityName { get { return InnerObject.commodityName; } }

        public string commodityPrice { get { return InnerObject.commodityPrice.ToString(); } }

        public string commodityFreight { get { return InnerObject.commodityFreight.ToString(); } }
        public int sellQuantity { get { return InnerObject.sellQuantity; } }

        public string commodityState { get { return InnerObject.commodityState == 1 ? "上架" : "下架"; } }
         [NoRender]
        public DateTime SubmitTime { get { return InnerObject.submitTime; } }



        [NoRender]
        public string boor { get; set; }
    }
}
