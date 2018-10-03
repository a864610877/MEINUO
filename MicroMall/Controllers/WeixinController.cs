using Ecard.Models;
using Ecard.Services;
using MessageHandle;
using MicroMall.Models;
using MicroMall.Models.InvestGames;
using Microsoft.Practices.Unity;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using WxPayAPI;

namespace MicroMall.Controllers
{
    public class WeixinController : Controller
    {
        public static  string Token = ""; //WebConfigurationManager.AppSettings["WeixinToken"];//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static  string EncodingAESKey = ""; //WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static  string AppId = ""; //WebConfigurationManager.AppSettings["WeixinAppId"];//与微信公众账号后台的AppId设置保持一致，区分大小写。

        private readonly IUnityContainer _container;
        [Dependency]
        public ILog4netService logService { get; set; }
        //[Dependency]
        public ISetWeChatService SetWeChatService { get; set; }

        public SetWeChat SetWeChat { get; set; }
         [Dependency]
        public ISiteService ISiteService { get; set; }
        // GET: /InvestGame/

        public WeixinController(IUnityContainer container, ISetWeChatService setWeChatService)
        {
            //var config = System.Web.HttpContext.Current.Server.MapPath("~/log4net.config");
            ////string config = Path.GetDirectoryName(typeof(HomeController).Assembly.Location) + "\\log4net.config";
            //var repository = new FileInfo(config);
            //XmlConfigurator.Configure(repository);
            ////logService = new LogService();
            //logService = log4net.LogManager.GetLogger(typeof(HomeController));
            _container = container;
            //SetWeChatService = setWeChatService;
            //SetWeChat = SetWeChatService.GetById(1);
            //if (SetWeChat != null)
            //{
            //    Token = SetWeChat.token;
            //    AppId = SetWeChat.appID;
                
            //}
        }
        //验证
        //public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        //{
        //    var request = _container.Resolve<VerifyServerUrl>();
        //    request.Set(signature, timestamp, nonce, echostr);
        //    return Content(request.Save());
        //}

        ///// <summary>
        ///// 关注推送
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Index()
        //{
        //    try
        //    {
        //         StreamReader stream = new StreamReader(Request.InputStream);  
        //         string xml = stream.ReadToEnd();
        //         logService.Insert(xml);
        //         var request = _container.Resolve<ConcernPush>();
        //        return Content(request.Save(xml));
        //    }
        //    catch(Exception ex)
        //    {
        //        logService.Insert(ex);
        //    }
        //    return Content("");
        //}

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
            {
                return Content("参数错误！");
            }

            postModel.Token = Token;
            postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = AppId;//根据自己后台的设置保持一致

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            var maxRecordCount = 10;

            var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(Request.InputStream, SetWeChat, _container, maxRecordCount);
            try
            {

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;


                //执行微信处理过程
                messageHandler.Execute();

                //测试时可开启，帮助跟踪数据

                if (messageHandler.ResponseDocument == null)
                {
                    throw new Exception(messageHandler.RequestDocument.ToString());
                }

                if (messageHandler.ResponseDocument != null)
                {
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                if (messageHandler.UsingEcryptMessage)
                {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}.txt", DateTime.Now.Ticks, messageHandler.RequestMessage.FromUserName)));
                }

                return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                //return new WeixinResult(messageHandler);//v0.8+
            }
            catch (Exception ex)
            {
                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + DateTime.Now.Ticks + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                }
                return Content("");
            }
        }
        /// <summary>
        /// 生成公众号按钮
        /// </summary>
        /// <returns></returns>
        public ActionResult Menus()
        {
            var request = _container.Resolve<CreateMenu>();
            return Content(request.MpCreate());
        }
        //[HttpPost]
        //public ActionResult WebConfing(string url)
        //{
        //    var request = _container.Resolve<SetWebConfing>();
        //    request.url=url;
        //    request.Set();
            
        //    return Json(request);
        //}

        [HttpPost]
        public ActionResult OAuth2(string state)
        {
            try
            {
                //构造网页授权获取code的URL
                //string host = page.Request.Url.Host;
                //string path = page.Request.Path;
                string redirect_uri = Server.UrlEncode(WxPayConfig.USER_NOTIFY_URL);
                WxPayAPI.Log.Debug(this.GetType().ToString(), WxPayConfig.USER_NOTIFY_URL);
                //WxPayData data = new WxPayData();
                //data.SetValue("appid", WxPayConfig.APPID);
                //data.SetValue("redirect_uri", redirect_uri);
                //data.SetValue("response_type", "code");
                //data.SetValue("scope", "snsapi_userinfo");
                ////data.SetValue("state", "STATE" + "#wechat_redirect");
                //data.SetValue("state", state + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+WxPayConfig.APPID+"&redirect_uri="+redirect_uri+"&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect";
                WxPayAPI.Log.Debug(this.GetType().ToString(),"授权url："+ url);
                return Json(new ResultMessage() { Code = 0, Msg = url });
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug(this.GetType().ToString(),ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg ="参数错误，请联系管理员！" });
            }
        }
        public ActionResult OAuth2url(string orderNo)
        {
            try
            {
                var site = ISiteService.Query(new SiteRequest()).FirstOrDefault();
                string url = site.Url + "Login/OrderLogin/";
                //构造网页授权获取code的URL
                //string host = page.Request.Url.Host;
                //string path = page.Request.Path;
                string redirect_uri = Server.UrlEncode(WxPayConfig.USER_NOTIFY_URL);
                WxPayAPI.Log.Debug(this.GetType().ToString(), WxPayConfig.USER_NOTIFY_URL);
                //WxPayData data = new WxPayData();
                //data.SetValue("appid", WxPayConfig.APPID);
                //data.SetValue("redirect_uri", redirect_uri);
                //data.SetValue("response_type", "code");
                //data.SetValue("scope", "snsapi_userinfo");
                ////data.SetValue("state", "STATE" + "#wechat_redirect");
                //data.SetValue("state", state + "#wechat_redirect");

                string url1 = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxPayConfig.APPID + "&redirect_uri=" + url + "&response_type=code&scope=snsapi_userinfo&state=" + orderNo + "#wechat_redirect";
                WxPayAPI.Log.Debug(this.GetType().ToString(), "授权url：" + url1);
                return Redirect(url1);
                //return Json(new ResultMessage() { Code = 0, Msg = url1 });
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug(this.GetType().ToString(), ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "参数错误，请联系管理员！" });
            }
        }

        public ActionResult OAuth2Register(string state)
        {
            try
            {
                //构造网页授权获取code的URL
                //string host = page.Request.Url.Host;
                //string path = page.Request.Path;
                string redirect_uri = Server.UrlEncode(WxPayConfig.USERRegister_NOTIFY_URL);
                WxPayAPI.Log.Debug(this.GetType().ToString(), WxPayConfig.USERRegister_NOTIFY_URL);
                //WxPayData data = new WxPayData();
                //data.SetValue("appid", WxPayConfig.APPID);
                //data.SetValue("redirect_uri", redirect_uri);
                //data.SetValue("response_type", "code");
                //data.SetValue("scope", "snsapi_userinfo");
                ////data.SetValue("state", "STATE" + "#wechat_redirect");
                //data.SetValue("state", state + "#wechat_redirect");
                string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxPayConfig.APPID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect";
                WxPayAPI.Log.Debug(this.GetType().ToString(), "授权url：" + url);
                return Json(new ResultMessage() { Code = 0, Msg = url });
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Debug(this.GetType().ToString(), ex.Message);
                return Json(new ResultMessage() { Code = -1, Msg = "参数错误，请联系管理员！" });
            }
        }
        [HttpPost]
        public ActionResult WebConfing(string url)
        {
            var request = _container.Resolve<SetWebConfing>();
            request.url = url;
            request.Set();
            return Json(request);
        }

        //public ActionResult CreateQrCode()
        //{
        //    //var request = _container.Resolve<CreateMenu>();
        //    //return Content(request.CreateQrCode());
        //}

    }
}
