using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Requests
{
    public class AdRequest : PageRequest
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? state { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startSubmitTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endSubmitTime { get; set; }
    }
}
