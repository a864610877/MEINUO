using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class JuMeiMallSearchRequest : PageRequest
    {
        public string commodityName { get; set; }

        public int commodityCategoryId { get; set; }
    }
}
