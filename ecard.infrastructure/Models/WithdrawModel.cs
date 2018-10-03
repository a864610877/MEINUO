using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class WithdrawModel : Withdraw
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        //public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
