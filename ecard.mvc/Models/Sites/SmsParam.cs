using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Ecard.Services;
using Ecard.Models;
using Ecard.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Models.Sites
{
    public class SmsParam
    {
        [Dependency, NoRender]
        public ISiteService SiteService { get; set; }
        [Dependency, NoRender]
        public Site Site { get; set; }
        [Dependency, NoRender]
        public ICacheService CacheService { get; set; }
        Site GetSite()
        {
            return Site ?? InnerDto;
        }
        [NoRender]
        public Site InnerDto { get; set; }
        public SmsParam()
        {
            InnerDto = new Site();
        }
        /// <summary>
        /// 短信的发送帐号
        /// </summary>
        public string SmsAccount { get; set; }
        /// <summary>
        /// 短信发送帐号的密码
        /// </summary>
        [DataType(DataType.Password)]
        public string SmsPwd { get; set; }
        //[Range(1,60,ErrorMessage="间隔只能在1-60秒之间")]
        //public int SmsTimeSpan { get; set; }
        public int RetryCount { get; set; }
        /// <summary>
        /// 剩余短信数量
        /// </summary>
        public string Unuse { get; private set; }
        ///// <summary>
        ///// 已经发送的数量
        ///// </summary>
        //public string Sends { get; private set; }
        public void Ready()
        {
            //获取短信的使用情况
            //http://kltx.sms10000.com.cn/sdk/SMS?cmd=getnum&uid=0001&psw=12345
            //if (!string.IsNullOrWhiteSpace(Site.SmsAccount))
            //{
            //    var str = SmsSendHelper.SendSms(SmsSendHelper.SmsUrl, string.Format(SmsSendHelper.QueryTemplate, Site.SmsAccount, SmsSendHelper.GetMd532(Site.SmsPwd??"")));
            //    var s = str.Split('#');
            //    if (s.Length > 1)
            //    {
            //        Unuse = s[2];
            //        Sends = s[1];
            //    }
            //    else { Unuse = "获取失败"; Sends = "获取失败"; }
            //}
            SmsPwd = Site.SmsPwd;
            SmsAccount = Site.SmsAccount;
           // SmsTimeSpan = Site.SmsTimeSpan;
            RetryCount = Site.RetryCount;

            System.Text.Encoding myEncode = System.Text.Encoding.GetEncoding("UTF-8");

            //以下参数为所需要的参数，测试时请修改
            string strReg = Site.SmsAccount;  //注册号（由华兴软通提供）
            string strPwd = Site.SmsPwd ?? "";                //密码（由华兴软通提供）

            string url = "http://www.stongnet.com/sdkhttp/getbalance.aspx";  //华兴软通查询余额地址

            //要发送的内容
            string strSend = "reg=" + strReg + "&pwd=" + strPwd;

            //发送
            string strRes = HttpSend.postSend(url, strSend);

            string[] strRess = strRes.Split('&');
            if (strRess.Length >= 2)
            {
                string[] restss = strRess[0].Split('=');
                if (restss.Length >= 2)
                {
                    if (restss[1] == "0")
                    {
                        string[] yes = strRess[1].Split('=');
                        if (yes.Length >= 2)
                        {
                            Unuse = yes[1];
                        }
                    }
                    else
                    {
                        Unuse = "获取失败";
                    }
                }
            }

            //string[] strs=strRes.Split('');
           

        }

        public void Save()
        {
            Site.SmsAccount = SmsAccount;
            if (!string.IsNullOrWhiteSpace(SmsPwd))
                Site.SmsPwd = SmsPwd;
            //Site.SmsTimeSpan = SmsTimeSpan;
            Site.RetryCount = RetryCount;
            SiteService.Update(Site);
            CacheService.Refresh(CacheKeys.SiteKey);
        }
    }
}
