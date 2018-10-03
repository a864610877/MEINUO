using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class MyCode
    {
        public string Id { get; set; }

        public int UserId { get; set; }

        public string CodeUrl { get; set; }

        public DateTime CreateTime { get; set; }
    }
}