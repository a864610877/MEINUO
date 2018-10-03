using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models
{
    public class SessionKeys
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public const string USERID = "USERID";
        /// <summary>
        /// 密码错误次数
        /// </summary>
        public const string PASSWORDERRORCOUNT = "PASSWORDERRORCOUNT";
        /// <summary>
        /// 第一次错误时间
        /// </summary>
        public const string FIRSPASSWORDERRORTIME = "FIRSPASSWORDERRORTIME";
    }
}