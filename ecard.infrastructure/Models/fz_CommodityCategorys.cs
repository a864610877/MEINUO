using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class fz_CommodityCategorys
    {
        [Key]
        public int commodityCategoryId { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
