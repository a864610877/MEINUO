using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.OperationPointLogs
{
    public class OperationPointLogList
    {
        private readonly OperationPointLogModel _innerObject;
        [NoRender]
        public OperationPointLogModel InnerObject
        {
            get { return _innerObject; }
        }

        public OperationPointLogList()
        {
            _innerObject = new OperationPointLogModel();
        }

        public OperationPointLogList(OperationPointLogModel innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int operationPointLogId { get { return InnerObject.operationPointLogId; } }

        public decimal point { get { return InnerObject.point; } }

        public string account { get { return InnerObject.account; } }
        public string DisplayName { get { return InnerObject.DisplayName; } }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get { return InnerObject.Mobile; } }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get { return InnerObject.Gender == 1 ? "男" : "女"; } }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get { return InnerObject.Email; } }

        public string remark { get { return InnerObject.remark; } }

        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }



        [NoRender]
        public string boor { get; set; }
    }
}
