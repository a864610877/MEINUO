using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 省
    /// </summary>
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }

        public string Name { get; set; }
    }
}
