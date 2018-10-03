using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class OrderDetailRequest : PageRequest
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
    }
}
