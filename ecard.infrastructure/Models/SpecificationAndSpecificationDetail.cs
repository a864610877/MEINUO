using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class SpecificationAndSpecificationDetail 
    {
        /// <summary>
        /// 实体
        /// </summary>
        public Specification model { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<SpecificationDetail> list { get; set; }
    }
}
