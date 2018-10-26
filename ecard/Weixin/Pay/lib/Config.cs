using Ecard;
using Ecard.Services;
using Ecard.SqlServices;
using Microsoft.Practices.Unity;
using Moonlit.Data;
using System;
using System.Collections.Generic;
using System.Web;

namespace WxPayAPI
{
    /**
    * 	配置账号信息
    */
    public static class WxPayConfig
    {
        static WxPayConfig()
        {
           var _databaseInstance = new DatabaseInstance(new Database("ecard"));
           var ISetWeChatService = new SqlSetWeChatService(_databaseInstance);
            var item = ISetWeChatService.GeByFirst();
            if (item != null)
            {
                //WxPayAPI.Log.Debug("WxPayAPI.WxPayConfig", "回调url：" + WxPayConfig.USER_NOTIFY_URL);
                APPID = item.appID;
                MCHID = item.MCHID;
                KEY = item.MCHIDKEY;
                APPSECRET = item.AppSecret;
                NOTIFY_URL = item.NOTIFY_URL;
                USER_NOTIFY_URL = item.USER_NOTIFY_URL;
                USERRegister_NOTIFY_URL = item.USERRegister_NOTIFY_URL;
                TOKEN = item.token;
            }
        }
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static string APPID = "wx900e7cc5bb31bfc9"; //开发者ID
        public static string MCHID = "1483978602"; //商户号
        public static string KEY = "22a2663ce8cdb17da1ee34920291a2be";//支付密码22a2663ce8cdb17da1ee34920291a2bd
        public static string APPSECRET = "fc18415a53c0c4429b2dc7b84500a961";//开发者密码

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public static string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public static string SSLCERT_PASSWORD = "1483978602";//商户号
        public static string TOKEN = "";


        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public static string NOTIFY_URL = "http://wx.fislive.com/WeChat/ResultNotifyPage.aspx";
        /// <summary>
        /// 用户授权回调url
        /// </summary>
        public static string USER_NOTIFY_URL = "http://wx.fislive.com/login/login";
        /// <summary>
        /// 用户注册回调地址
        /// </summary>
        public static string USERRegister_NOTIFY_URL = "";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public static string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public static string PROXY_URL = "http://192.168.0.251:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public static int REPORT_LEVENL = 0;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public static int LOG_LEVENL = 3;
    }
}