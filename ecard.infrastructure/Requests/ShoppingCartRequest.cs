using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class ShoppingCartRequest : PageRequest
    {
        public int? UserId { get; set; }
    }
}
