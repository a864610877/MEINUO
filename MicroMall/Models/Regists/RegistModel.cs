using MicroMall.Models.layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Regists
{
    public class RegistModel 
    {
        public string orangeKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 省id
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string PasswordConfirm { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }


    }
}