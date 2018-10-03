using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class JuMeiMallModelExpress: JuMeiMallModel
    {
        public decimal commodityJifen { get; set; }
    }

    public class JuMeiMallModel
    {
        public int commodityId { get; set; }
        public string images { get; set; }
        public decimal commodityPrice { get; set; }

        public string commodityName { get; set; }
        public int sellQuantity { get; set; }
        
        public int commodityRank { get; set; }

        public string Categoryname { get; set; }

        public int commodityCategoryId { get; set; }
        public string submitTime { get; set; }

        public string commodityRemark { get; set; }
    }

    public class CateMallModel{
        public int commodityCategoryId { get; set; }
        public string Categoryname { get; set; }
    
    }


}