using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models
{
    public class ThModel
    {
        public string Text { get; set; }
        public string Sort { get; set; }
        public bool? Directory { get; set; }
        public bool Sortable { get { return !string.IsNullOrEmpty(Sort); } }
    }
}
