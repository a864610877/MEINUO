using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 规格详情
    /// </summary>
    public class SpecificationDetail
    {
        [Key]
        public int specificationDetailId { get; set; }
        /// <summary>
        /// 商品规格表ID
        /// </summary>
        public int specificationId { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime submitTime { get; set; }
    }
}
