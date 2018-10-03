using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Homes
{
    public class AdModel
    {

        public AdModel()
        {
            _ad = new Ad();
        }

        public AdModel(Ad model,string url)
        {
            _ad = model;
            _url = url;

        }
        public string _url { get; set; }
        private Ad _ad { get; set; }
        public string title { get { return _ad.title; } }

        public string link { get { return _ad.link; } }

        public string ImageUrl { get { return _url + "/AdImages/" + _ad.ImageUrl; } }

        //public string 
    }
}