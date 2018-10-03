using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Commoditys
{
    public class ReviewView
    {
        public int ReviewId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public string SubmitTime { get; set; }
    }

   public class  ListReviewView
   {
       public int TotalCount { get; set; }
       public int NextPage { get; set; }

       public List<ReviewView> List { get; set; }
   }
}