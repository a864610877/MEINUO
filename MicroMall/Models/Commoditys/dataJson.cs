using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Commoditys
{
    public class dataJson
    {
        public int count { get; set; }

        public int start { get; set; }

        public int total { get; set; }

        public int nextPage { get; set; }

        public List<CommodityModel> events { get; set; }
    } 
}