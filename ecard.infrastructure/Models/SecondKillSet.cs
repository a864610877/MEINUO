using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class SecondKillSet
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 0未开启 1开启
        /// </summary>
        [Bounded(typeof(SecondKillSetState))]
        public int state { get; set; }

        public DateTime? startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? endTime { get; set; }
    }

    public class SecondKillSetState
    {
        /// <summary>
        /// 正常的 1
        /// </summary>
        public const int Normal = 1;
        /// <summary>
        /// 无效的
        /// </summary>
        public const int Invalid = 2;
    }
}
