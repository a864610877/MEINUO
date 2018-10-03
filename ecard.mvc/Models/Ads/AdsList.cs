using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Ads
{
    public class AdsList
    {


        private readonly Ad _innerObject;
        [NoRender]
        public Ad InnerObject
        {
            get { return _innerObject; }
        }

        public AdsList()
        {
            _innerObject = new Ad();
        }

        public AdsList(Ad innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int adId { get { return InnerObject.adId; } }

        public string title { get { return InnerObject.title; } }
        public string link { get { return InnerObject.link; } }
        public string state { get { return InnerObject.State == 1 ? "上架" : "下架"; } }
        public int rank { get { return InnerObject.rank; } }
        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }

        [NoRender]
        public string boor { get; set; }
    }
}
