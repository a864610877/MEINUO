using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class ReviewRequest : PageRequest
    {
        public int? UserId { get; set; }

        public int? CommodityId { get; set; }

        public int? State { get; set; }

        public string UserName { get; set; }

        public string CommodityNo { get; set; }
    }
}
