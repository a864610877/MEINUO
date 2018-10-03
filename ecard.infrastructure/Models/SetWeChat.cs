using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    public class SetWeChat
    {
        [Key]
        public int setWeChatId { get; set; }
        /// <summary>
        /// 公众号APPid
        /// </summary>
        public string appID { get; set; }
        /// <summary>
        /// 公众帐号secert（仅JSAPI支付的时候需要配置）
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }

        public string access_token { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MCHID { get; set; }
        /// <summary>
        /// 商户支付密钥
        /// </summary>
        public string MCHIDKEY { get; set; }
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string NOTIFY_URL { get; set; }
        /// <summary>
        /// 用户授权回调url
        /// </summary>
        public string USER_NOTIFY_URL { get; set; }
        /// <summary>
        /// 用户注册回调url
        /// </summary>
        public string USERRegister_NOTIFY_URL { get; set; }

        public DateTime? overtime { get; set; }
    }
}
