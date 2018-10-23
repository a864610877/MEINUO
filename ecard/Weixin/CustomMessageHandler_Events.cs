using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.Context;
using System.IO;
using System.Diagnostics;
using System.Web;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Agent;
using System.Web.Configuration;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Ecard.Models;
using Ecard;
using WxPayAPI;

namespace MessageHandle
{
    public partial class CustomMessageHandler : MessageHandler<MessageContext<IRequestMessageBase, IResponseMessageBase>>
    {
        private string appId = "wx7f20e84eeb8b681e"; //WebConfigurationManager.AppSettings["appId"];
        private string appSecret = "9e9780530d96c9c0799a914e34c3e478"; //WebConfigurationManager.AppSettings["appSecret"];

        private string agentUrl ="";// WebConfigurationManager.AppSettings["WeixinAgentUrl"];//这里使用了www.weiweihi.com微信自动托管平台
        private string agentToken = "110buhaoma"; //WebConfigurationManager.AppSettings["WeixinAgentToken"];//Token
        private string wiweihiKey ="";// WebConfigurationManager.AppSettings["WeixinAgentWeiweihiKey"];//WeiweihiKey专门用于对接www.Weiweihi.com平台，获取方式见：http://www.weiweihi.com/ApiDocuments/Item/25#51
        private  IUnityContainer _container;
        private string GetWelcomeInfo()
        {
            //获取Senparc.Weixin.MP.dll版本信息
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/Senparc.Weixin.MP.dll"));
            var version = string.Format("{0}.{1}", fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart);
            return string.Format(
@"欢迎关注【Senparc.Weixin.MP 微信公众平台SDK】，当前运行版本：v{0}。
您可以发送【文字】【位置】【图片】【语音】等不同类型的信息，查看不同格式的回复。

您也可以直接点击菜单查看各种类型的回复。
还可以点击菜单体验微信支付。

SDK官方地址：http://weixin.senparc.com
源代码及Demo下载地址：https://github.com/JeffreySu/WeiXinMPSDK
Nuget地址：https://www.nuget.org/packages/Senparc.Weixin.MP",
                version);
        }

        public string msg()
        {
            return "<a href='#'><div class='message_wrap' style='box-sizing:border-box; padding:15px; margin:15px;background:#fff;  -webkit-box-shadow:0 0 3px #ccc;  -moz-box-shadow:0 0 3px #ccc;  box-shadow:0 0 3px #ccc;'><header><h2 style='font-size:14px; font-family:'微软雅黑'; font-weight: normal;color:#000; line-height:18px; max-height:36px;text-overflow: -o-ellipsis-lastline;overflow: hidden;text-overflow: ellipsis;display: -webkit-box;-webkit-line-clamp: 2;-webkit-box-orient: vertical;'>今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早</h2><!-------40字以内--------><p>8月4日</p></header><section >  <img src='images/messgaesimg.jpg' width='100%;'  height='40px'/> </section><footer><p style='color:#999; margin-top:10px; font-size:12px; font-family:'微软雅黑'; line-height:18px;'>今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早今天起得早</p>  <!-------100字以内--------> <h3 style='font-size:14px; margin-top:15px;'>查看原文</h3></footer></div></a>";
        }

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey

            if (requestMessage.Content == "OneClick")
            {
                var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                return strongResponseMessage;
            }
            else if (requestMessage.Content == "Register")
            {
                //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);
                //responseMessage.Articles.Add(new Article()
                //{
                //    Title = "速度，加入雷鹏汽车吧！",
                //    Description = "注册，成为会员即赠送积分！",
                //    PicUrl = "http://shop.leipengcar.com/MsgImages/one.jpg",//requestMessage.PicUrl,
                //    Url = "http://shop.leipengcar.com/Regists/Regist?openId=" + requestMessage.FromUserName
                //});
                //return responseMessage;
            }
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配
            switch (requestMessage.EventKey)
            {
                case "OneClick":
                    {
                        //这个过程实际已经在OnTextOrEventRequest中完成，这里不会执行到。
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了底部按钮。\r\n为了测试微信软件换行bug的应对措施，这里做了一个——\r\n换行";
                    }
                    break;
                case "SubClickRoot_Text":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了子菜单按钮。";
                    }
                    break;
                case "SubClickRoot_News":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Articles.Add(new Article()
                        {
                            Title = "您点击了子菜单图文按钮",
                            Description = "您点击了子菜单图文按钮，这是一条图文信息。",
                            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg",
                            Url = "http://weixin.senparc.com"
                        });
                    }
                    break;
                case "SubClickRoot_Music":
                    {
                        ////上传缩略图
                        //var accessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetToken(appId, appSecret);
                        //var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.Media.MediaApi.UploadTemporaryMedia(accessToken, UploadMediaFileType.thumb,
                        //                                             Server.GetMapPath("~/Images/Logo.jpg"));
                        ////设置音乐信息
                        //var strongResponseMessage = CreateResponseMessage<ResponseMessageMusic>();
                        //reponseMessage = strongResponseMessage;
                        //strongResponseMessage.Music.Title = "天籁之音";
                        //strongResponseMessage.Music.Description = "真的是天籁之音";
                        //strongResponseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                        //strongResponseMessage.Music.HQMusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
                        //strongResponseMessage.Music.ThumbMediaId = uploadResult.thumb_media_id;
                    }
                    break;
                case "SubClickRoot_Image":
                    {
                        ////上传图片
                        //var accessToken = Senparc.Weixin.MP.CommonAPIs.AccessTokenContainer.TryGetToken(appId, appSecret);
                        //var uploadResult = Senparc.Weixin.MP.AdvancedAPIs.Media.MediaApi.UploadTemporaryMedia(accessToken, UploadMediaFileType.image,
                        //                                             Server.GetMapPath("~/Images/Logo.jpg"));
                        ////设置图片信息
                        //var strongResponseMessage = CreateResponseMessage<ResponseMessageImage>();
                        //reponseMessage = strongResponseMessage;
                        //strongResponseMessage.Image.MediaId = uploadResult.media_id;
                    }
                    break;
                case "SubClickRoot_Agent"://代理消息
                    {
                        //获取返回的XML
                        DateTime dt1 = DateTime.Now;
                        reponseMessage = MessageAgent.RequestResponseMessage(this, agentUrl, agentToken, RequestDocument.ToString());
                        //上面的方法也可以使用扩展方法：this.RequestResponseMessage(this,agentUrl, agentToken, RequestDocument.ToString());

                        DateTime dt2 = DateTime.Now;

                        if (reponseMessage is ResponseMessageNews)
                        {
                            (reponseMessage as ResponseMessageNews)
                                .Articles[0]
                                .Description += string.Format("\r\n\r\n代理过程总耗时：{0}毫秒", (dt2 - dt1).Milliseconds);
                        }
                    }
                    break;
                case "Member"://托管代理会员信息
                    {
                        //原始方法为：MessageAgent.RequestXml(this,agentUrl, agentToken, RequestDocument.ToString());//获取返回的XML
                        reponseMessage = this.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
                    }
                    break;
                case "OAuth"://OAuth授权测试
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        strongResponseMessage.Articles.Add(new Article()
                        {
                            Title = "OAuth2.0测试",
                            Description = "点击【查看全文】进入授权页面。\r\n注意：此页面仅供测试（是专门的一个临时测试账号的授权，并非Senparc.Weixin.MP SDK官方账号，所以如果授权后出现错误页面数正常情况），测试号随时可能过期。请将此DEMO部署到您自己的服务器上，并使用自己的appid和secret。",
                            Url = "http://weixin.senparc.com/oauth2",
                            PicUrl = "http://weixin.senparc.com/Images/qrcode.jpg"
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "Description":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = GetWelcomeInfo();
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "SubClickRoot_PicPhotoOrAlbum":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了【微信拍照】按钮。系统将会弹出拍照或者相册发图。";
                    }
                    break;
                case "SubClickRoot_ScancodePush":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "您点击了【微信扫码】按钮。";
                    }
                    break;
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        public override IResponseMessageBase OnEvent_EnterRequest(RequestMessageEvent_Enter requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
            return responseMessage;//这里也可以返回null（需要注意写日志时候null的问题）
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //通过扫描关注
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            var AccountService = _container.Resolve<IAccountService>();
            var MembershipService = _container.Resolve<IMembershipService>();
            var TransactionHelper = _container.Resolve<TransactionHelper>();
            var RecommendLogService = _container.Resolve<IRecommendLogService>();
            var item = AccountService.GetByopenID(requestMessage.FromUserName);
            var userWX=Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(WxPayConfig.APPID, requestMessage.FromUserName);
            if (item == null)
            {
                string orangeKey = requestMessage.EventKey.Replace("qrscene_", "");
                int salerId = 0;
                Account saleAccont = null;
                if (!string.IsNullOrWhiteSpace(orangeKey))
                {
                    saleAccont = AccountService.GetByorangeKey(orangeKey);
                    if (saleAccont == null)
                    {
                    }
                    else
                     salerId = saleAccont.accountId;
                }
                TransactionHelper.BeginTransaction();
                AccountUser modelUser = new AccountUser();
                modelUser.Address ="" ;
                modelUser.DisplayName = userWX.nickname;
                modelUser.Email = "";
                modelUser.Gender = userWX.sex;
                modelUser.Mobile = "";
                modelUser.Name = requestMessage.FromUserName;
                modelUser.Photo = userWX.headimgurl.Replace("/0", "/132");
                //modelUser.SetPassword(Password);
                modelUser.State = UserStates.Normal;
                MembershipService.CreateUser(modelUser);
                Account modelAccount = new Account();
                //var QrCodeResult = CreateQrCode();
                //modelAccount.ticket = QrCodeResult.ticket;
                modelAccount.activatePoint = 0;
                //modelAccount.notActivatePoint = 0;
                modelAccount.orangeKey = modelUser.UserId.ToString().PadLeft(modelUser.UserId.ToString().Length + 2, '0');
                //modelAccount.payPoint = 0;
                modelAccount.openID = requestMessage.FromUserName;
                //modelAccount.presentExp = site.givePoint;
                modelAccount.salerId = salerId;
                modelAccount.state = AccountStates.Normal;
                modelAccount.submitTime = DateTime.Now;
                modelAccount.userId = modelUser.UserId;
                modelAccount.grade = AccountGrade.not;
                AccountService.Insert(modelAccount);
                if (salerId > 0)
                {
                    RecommendLog recommendlog = new RecommendLog();
                    recommendlog.remark = string.Format("推荐了{0}", userWX.nickname);
                    recommendlog.salerId = saleAccont.userId;
                    recommendlog.submitTime = DateTime.Now;
                    recommendlog.userId = modelUser.UserId;
                    recommendlog.userName = requestMessage.FromUserName;
                    RecommendLogService.Insert(recommendlog);
                    string msg = string.Format("恭喜您,您成功推荐了【{0}】成为了您的直属粉丝，时间：{1}", userWX.nickname, DateTime.Now);
                    Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(WxPayConfig.APPID, requestMessage.FromUserName, msg);
                }
                responseMessage.Content = "快去成为会员吧";
                //responseMessage.Articles.Add(new Article()
                //{
                //    Title = "速度，加入雷鹏汽车吧！",
                //    Description = "注册，成为会员即赠送积分！",
                //    PicUrl = "http://shop.leipengcar.com/MsgImages/one.jpg",//requestMessage.PicUrl,
                //    Url = "http://shop.leipengcar.com/Regists/Regist?openId=" + requestMessage.FromUserName
                //});
            }
            else
            {

            }
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessage)
        {
            //说明：这条消息只作为接收，下面的responseMessage到达不了客户端，类似OnEvent_UnsubscribeRequest
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您点击了view按钮，将打开网页：" + requestMessage.EventKey;
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_MassSendJobFinishRequest(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "接收到了群发完成的信息。";
            return responseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            var AccountService = _container.Resolve<IAccountService>();
            var MembershipService = _container.Resolve<IMembershipService>();
            var TransactionHelper = _container.Resolve<TransactionHelper>();
            var RecommendLogService = _container.Resolve<IRecommendLogService>();
            var item = AccountService.GetByopenID(requestMessage.FromUserName);
            
            var userWX = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(WxPayConfig.APPID, requestMessage.FromUserName);
            if (item == null)
            {
                string orangeKey = requestMessage.EventKey.Replace("qrscene_", "");
                int salerId = 0;
                Account saleAccont = null;
                if (!string.IsNullOrWhiteSpace(orangeKey))
                {
                    saleAccont = AccountService.GetByorangeKey(orangeKey);
                    if (saleAccont == null)
                    {
                         
                    }
                    else
                        salerId = saleAccont.accountId;
                }
                TransactionHelper.BeginTransaction();
                AccountUser modelUser = new AccountUser();
                modelUser.Address = "";
                modelUser.DisplayName = userWX.nickname;
                modelUser.Email = "";
                modelUser.Gender = userWX.sex;
                modelUser.Mobile = "";
                modelUser.Name = requestMessage.FromUserName;
                modelUser.Photo = userWX.headimgurl.Replace("/0", "/132");
                //modelUser.SetPassword(Password);
                modelUser.State = UserStates.Normal;
                MembershipService.CreateUser(modelUser);
                Account modelAccount = new Account();
                //var QrCodeResult = CreateQrCode();
                //modelAccount.ticket = QrCodeResult.ticket;
                modelAccount.activatePoint = 0;
                //modelAccount.notActivatePoint = 0;
                modelAccount.orangeKey = modelUser.UserId.ToString().PadLeft(modelUser.UserId.ToString().Length + 2, '0');
                //modelAccount.payPoint = 0;
                modelAccount.openID = requestMessage.FromUserName;
                //modelAccount.presentExp = site.givePoint;
                modelAccount.salerId = salerId;
                modelAccount.state = AccountStates.Normal;
                modelAccount.submitTime = DateTime.Now;
                modelAccount.userId = modelUser.UserId;
                modelAccount.grade = AccountGrade.not;
                AccountService.Insert(modelAccount);
                if (salerId > 0)
                {
                    RecommendLog recommendlog = new RecommendLog();
                    recommendlog.remark = string.Format("推荐了{0}", userWX.nickname);
                    recommendlog.salerId = saleAccont.userId;
                    recommendlog.submitTime = DateTime.Now;
                    recommendlog.userId = modelUser.UserId;
                    recommendlog.userName = requestMessage.FromUserName;
                    RecommendLogService.Insert(recommendlog);
                    string msg = string.Format("恭喜您,您成功推荐了【{0}】成为了您的直属粉丝，时间：{1}", userWX.nickname,DateTime.Now);
                    Senparc.Weixin.MP.AdvancedAPIs.CustomApi.SendText(WxPayConfig.APPID, requestMessage.FromUserName, msg);
                }
                responseMessage.Content = "快去成为会员吧";
                //responseMessage.Articles.Add(new Article()
                //{
                //    Title = "速度，加入雷鹏汽车吧！",
                //    Description = "注册，成为会员即赠送积分！",
                //    PicUrl = "http://shop.leipengcar.com/MsgImages/one.jpg",//requestMessage.PicUrl,
                //    Url = "http://shop.leipengcar.com/Regists/Regist?openId=" + requestMessage.FromUserName
                //});
            }
            else
            {
               
            }

           
            //responseMessage.Content = msg();// "欢迎关注雷鹏汽车用品！​";//GetWelcomeInfo();
            return responseMessage;
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodePushRequest(RequestMessageEvent_Scancode_Push requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            
            responseMessage.Content = "事件之扫码推事件";
            return responseMessage;
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之扫码推事件且弹出“消息接收中”提示框";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出拍照或者相册发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicSysphotoRequest(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出系统拍照发图";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_PicWeixinRequest(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出微信相册发图器";
            return responseMessage;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出地理位置选择器";
            return responseMessage;
        }
    }

    public static class Server
    {
        private static string _appDomainAppPath;
        public static string AppDomainAppPath
        {
            get
            {
                if (_appDomainAppPath == null)
                {
                    _appDomainAppPath = HttpRuntime.AppDomainAppPath;
                }
                return _appDomainAppPath;
            }
            set
            {
                _appDomainAppPath = value;
            }
        }

        public static string GetMapPath(string virtualPath)
        {
            if (virtualPath == null)
            {
                return "";
            }
            else if (virtualPath.StartsWith("~/"))
            {
                return virtualPath.Replace("~/", AppDomainAppPath).Replace("/", "\\");
            }
            else
            {
                return Path.Combine(AppDomainAppPath, virtualPath.Replace("/", "\\"));
            }
        }

        public static HttpContext HttpContext
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                {
                    HttpRequest request = new HttpRequest("Default.aspx", "http://weixin.senparc.com/default.aspx", null);
                    StringWriter sw = new StringWriter();
                    HttpResponse response = new HttpResponse(sw);
                    context = new HttpContext(request, response);
                }
                return context;
            }
        }
    }
}
