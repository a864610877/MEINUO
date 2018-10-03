using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class OperationPointLogModel
    {
        [NoRender]
        public int operationPointLogId { get; set; }

        public decimal point { get; set; }

        public string account { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        public string remark { get; set; }

        public DateTime submitTime { get; set; }

    }
}
