using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ecard.Models;

namespace Oxite.Model
{
    public class WebPart : EntityBase
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
    public class WebPartItem
    {
        public string Range { get; set; }
        public WebPart WebPart { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }

        public object CreateRouteDictionary()
        {
            return new { controller = this.WebPart.Controller, action = this.WebPart.Action };
        }
    }
}
