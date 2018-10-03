using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MicroMall.Models.InvestGames
{
    public class VerifyServerUrl
    {

        public void Set(string signature,string timestamp,string nonce,string echostr)
        {
            this.echostr = echostr;
            this.nonce = nonce;
            this.signature = signature;
            this.timestamp = timestamp;
        }
        /// <summary>
        /// 
        /// </summary>
        [Dependency]
        public ILog4netService logService { get; set; }
        [Dependency]
        public ISetWeChatService SetWeChatService { get; set; }
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string echostr { get; set; }

        public string Save()
        {
            try
            {
                logService.Insert(string.Format("--开始：验证服务器配置--"));
                logService.Insert(string.Format("参数-signature:{0},timestamp:{1},nonce:{1},echostr:{2}", signature, timestamp, nonce, echostr));
                var setWeChat = SetWeChatService.GetById(1);
                if (setWeChat == null)
                {
                    logService.Insert("未设置配置信息！");
                    return "";
                    //return new ResponseResult() { code = -1, codeText = "" };
                }
                string token = setWeChat.token;
                string[] item = { token, timestamp, nonce };
                Array.Sort(item);
                string tmpStr = string.Join("", item);
                tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
                tmpStr = tmpStr.ToLower();
                logService.Insert(string.Format("匹配密钥：{0}", tmpStr));
                if (tmpStr == signature)
                {
                    logService.Insert(string.Format("验证结果：{0}", "成功"));
                    return echostr;
                }
                logService.Insert(string.Format("验证结果：{0}", "失败"));
                return "";
            }
            catch (Exception ex)
            {
                logService.Insert(ex);
                return "";
            }
        }
    }
}