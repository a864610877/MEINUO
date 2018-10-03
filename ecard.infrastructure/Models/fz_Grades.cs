using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 级别设置 -- Matthew 20170419
    /// </summary>
    public class fz_Grades
    {
        /// <summary>
        /// 关联fz_Accounts gradeId
        /// </summary>
        [Key]
        public int gradeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal sale { get; set; }
    }
}
