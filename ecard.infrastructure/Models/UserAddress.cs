using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class UserAddress
    {
        [Key]
        public int userAddressId { get; set; }
        /// <summary>
        /// 管理用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string recipients { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string zipCode { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int provinceId { get; set; }
        /// <summary>
        /// 城市Id
        /// </summary>
        public int cityId { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string provinceName { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string cityName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string detailedAddress { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string moblie { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone { get; set; }
    }
}
