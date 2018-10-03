using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Reviews
{
    public class ListReview
    {
         private readonly Reviewss _innerObject;
        [NoRender]
         public Reviewss InnerObject
        {
            get { return _innerObject; }
        }

        public ListReview()
        {
            _innerObject = new Reviewss();
        }
        public ListReview(Reviewss innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int ReviewId { get { return InnerObject.ReviewId; } }

        public string commodityNo { get { return InnerObject.commodityNo; } }

        public string commodityName { get { return InnerObject.commodityName; } }

        public string userName { get { return InnerObject.UserName; } }

        public string content { get { return InnerObject.Content; } }

        public string state { get { return ModelHelper.GetBoundText(InnerObject, x => x.State); } }

        public DateTime submitTime { get { return InnerObject.SubmitTime; } }

       


    }
}
