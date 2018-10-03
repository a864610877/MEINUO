using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class RecommendLogModel
    {
        public int recommendLogId { get; set; }

        public string salerName { get; set; }

        public string saler { get; set; }
        public string salerphone { get; set; }

        public string userName { get; set; }
        public string user { get; set; }
        public string userphone { get; set; }

        public string remark { get; set; }

        public DateTime submitTime { get; set; }
    }
}
