using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.RecommendLogs
{
    public class RecommendLogList
    {
        private readonly RecommendLogModel _innerObject;
        [NoRender]
        public RecommendLogModel InnerObject
        {
            get { return _innerObject; }
        }

        public RecommendLogList()
        {
            _innerObject = new RecommendLogModel();
        }

        public RecommendLogList(RecommendLogModel innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int recommendLogId { get { return InnerObject.recommendLogId; } }
        [NoRender]
        public string salerName { get { return InnerObject.salerName; } }

        public string saler { get { return InnerObject.saler; } }
        public string salerphone { get { return InnerObject.salerphone; } }
        [NoRender]
        public string userName { get { return InnerObject.userName; } }
        public string user { get { return InnerObject.user; } }
        public string userphone { get { return InnerObject.userphone; } }

        public string remark { get { return InnerObject.remark; } }

        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }



        [NoRender]
        public string boor { get; set; }
    }
}
