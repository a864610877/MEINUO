using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models
{
    public class ResultMessage : QuantityMessage
    {
        public int Code { get; set; }

        public string Msg { get; set; }


    }

    public class QuantityMessage 
    {
        public int quantity { get; set; }
    }


}