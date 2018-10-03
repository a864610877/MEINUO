using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace MicroMall.Models.InvestGames
{
    public class SetWebConfing:layouts.LayoutModel
    {
        /// <summary>
        ///  必填，公众号的唯一标识
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 必填，生成签名的时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 必填，生成签名的随机串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 必填，签名，见附录1
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 分享图片
        /// </summary>
        public string ImageUrl { get; set; }

        public string url { get; set; }

        public void Set()
        {
           // Load();
            //if(Account!=null)
            //{
            //    ImageUrl = Account.qrCodeUrl;
            //}
            //appId = WxPayConfig.APPID;
           // var access_token = AccessTokenContainer.GetToken(WxPayConfig.APPID);
            var jsapi_ticket = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetTicket(WxPayConfig.APPID, WxPayConfig.APPSECRET).ticket;
            timestamp = WxPayApi.GenerateTimeStamp();
            nonceStr = WxPayApi.GenerateNonceStr();
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("noncestr", nonceStr);
            jsApiParam.SetValue("jsapi_ticket", jsapi_ticket);
            jsApiParam.SetValue("timestamp", timestamp);
            jsApiParam.SetValue("url", url);
            signature = jsApiParam.SHA1_Hash();
        }
    }
}