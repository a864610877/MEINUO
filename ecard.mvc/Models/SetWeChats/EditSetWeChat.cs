using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.SetWeChats
{
    public class EditSetWeChat : EcardModelListRequest<SetWeChat>
    {


        [Dependency]
        [NoRender]
        public ISetWeChatService setWeChatService { get; set; }

        public int setWeChatId { get; set; }
        public string appID { get; set; }

        public string AppSecret { get; set; }

        public string token { get; set; }

        public string access_token { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MCHID { get; set; }
        /// <summary>
        /// 商户支付密钥
        /// </summary>
        public string KEY { get; set; }
        /// <summary>
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string NOTIFY_URL { get; set; }
        /// <summary>
        /// 用户授权回调url
        /// </summary>
        public string USER_NOTIFY_URL { get; set; }
        /// <summary>
        /// 用户注册url
        /// </summary>
        public string USERRegister_NOTIFY_URL { get; set; }

        public DateTime overtime { get; set; }
        public void Ready()
        {
            var chat = setWeChatService.GetById(1);
            this.setWeChatId = chat.setWeChatId;
            this.appID = chat.appID;
            this.AppSecret = chat.AppSecret;
            this.token = chat.token;
            this.access_token = chat.access_token;
            this.MCHID = chat.MCHID;
            this.KEY = chat.MCHIDKEY;
            this.NOTIFY_URL = chat.NOTIFY_URL;
            this.USER_NOTIFY_URL = chat.USER_NOTIFY_URL;
            this.USERRegister_NOTIFY_URL = chat.USERRegister_NOTIFY_URL;
        }

        public ResultMsg Save()
        {
            ResultMsg result = new ResultMsg();
            var chat = setWeChatService.GetById(this.setWeChatId);
            chat.appID = this.appID;
            chat.AppSecret = this.AppSecret;
            chat.token = this.token;
            chat.access_token = this.access_token;
            chat.MCHID = this.MCHID;
            chat.MCHIDKEY = this.KEY;
            chat.NOTIFY_URL = this.NOTIFY_URL;
            chat.USER_NOTIFY_URL = this.USER_NOTIFY_URL;
            chat.USERRegister_NOTIFY_URL=this.USERRegister_NOTIFY_URL;
            chat.overtime = DateTime.Now;
            try
            {
                setWeChatService.Update(chat);
                result.Code = 1;
                result.CodeText = "设置成功！";
                return result;
            }
            catch (Exception)
            {
                result.Code = 2;
                result.CodeText = "设置失败！";
                return result;
            }
        }
    }
}
