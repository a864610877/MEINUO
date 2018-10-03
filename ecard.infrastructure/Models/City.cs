using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 城市
    /// </summary>
    public class City
    {
        [Key]
        public int CityId { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
    }
}
