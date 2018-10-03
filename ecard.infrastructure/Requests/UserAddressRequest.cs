using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class UserAddressRequest : PageRequest
    {
        public int? UserId { get; set; }
    }
}
