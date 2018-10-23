using Ecard.Models;
using Ecard.Services;
using MicroMall.Models;
using Microsoft.Practices.Unity;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WxPayAPI;

namespace MicroMall.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [Dependency]
        public IAccountService IAccountService { get; set; }

        [Dependency]
        public IMembershipService MembershipService { get; set; }
        JsApiPay jsApiPay = new JsApiPay();
        public ActionResult Index()
        {
            HttpCookie cookie = new HttpCookie(SessionKeys.USERID, "1012");
            Response.Cookies.Add(cookie);
            Session[SessionKeys.USERID] = "1012";
            return RedirectToAction("index", "PersonalCentre");
            //string redirect_uri = Server.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["USERLOGIN_NOTIFY_URL"].ToString());
            //string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxPayConfig.APPID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=#wechat_redirect";
            //return Redirect(url);
        }


        [HttpPost]
        public ActionResult Login2(string mobile,string password)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return Json(new ResultMessage() { Code = -1, Msg = "请输入手机号!" });
            if (string.IsNullOrWhiteSpace(password))
                return Json(new ResultMessage() { Code = -1, Msg = "请输入密码!" });
            int errorCount = 0;
            DateTime errorTime = DateTime.Now;
            if (Session[mobile + "_" + SessionKeys.FIRSPASSWORDERRORTIME] != null)
                errorTime = Convert.ToDateTime(Session[mobile + "_" + SessionKeys.FIRSPASSWORDERRORTIME].ToString());
            if ((DateTime.Now - errorTime).TotalMinutes <= 30)
            {
                if (Session[mobile + "_" + SessionKeys.PASSWORDERRORCOUNT] != null)
                    errorCount = Convert.ToInt32(Session[mobile + "_" + SessionKeys.PASSWORDERRORCOUNT]);
                if (errorCount >= 5)
                    return Json(new ResultMessage() { Code = -1, Msg = "密码错误次数过多，请稍后在试!" });
            }
            else
            {
                errorCount = 0;
                Session[mobile + "_" + SessionKeys.PASSWORDERRORCOUNT] = errorCount;
            }

            var user = MembershipService.GetByMobile(mobile);
            if (user != null)
            {
                if (user.Password !=user.GetPassword(password))
                {
                    errorCount += 1;
                    if (Session[mobile+"_"+SessionKeys.FIRSPASSWORDERRORTIME] == null)
                        Session[mobile + "_" + SessionKeys.FIRSPASSWORDERRORTIME] = DateTime.Now;
                    Session[mobile + "_" + SessionKeys.PASSWORDERRORCOUNT] = errorCount;
                    return Json(new ResultMessage() { Code = -1, Msg = "密码错误!" });
                }
                var account = IAccountService.GetByUserId(user.UserId);
                if(account==null)
                    return Json(new ResultMessage() { Code = -1, Msg = "手机号异常，请联系管理员!" });

                HttpCookie cookie = new HttpCookie(SessionKeys.USERID, user.UserId.ToString());
                Response.Cookies.Add(cookie);
                Session[SessionKeys.USERID] = user.UserId;
                Session[mobile + "_" + SessionKeys.FIRSPASSWORDERRORTIME] =null;
                Session[mobile + "_" + SessionKeys.PASSWORDERRORCOUNT] = null;
                if (string.IsNullOrWhiteSpace(account.openID))
                {
                    //跳转到微信获取用户信息
                    string redirect_uri = Server.UrlEncode(WxPayConfig.USER_NOTIFY_URL);
                    string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxPayConfig.APPID + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_userinfo&state=#wechat_redirect";
                    return Json(new ResultMessage() { Code = -0, Msg = url });
                    //return Json(new ResultMessage() { Code = 0, Msg = "/PersonalCentre/Index" });
                }
                else
                {
                    //跳转到个人中心
                    return Json(new ResultMessage() { Code = 0, Msg = "/PersonalCentre/Index" });
                }
            }
            else
            {
                return Json(new ResultMessage() { Code = -1, Msg = "手机号不存在，请先注册后在登录!" });
            }
        }

        public ActionResult Login(string code,string state)
        {
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---进入了用户信息回调---"));
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---code:{0},state:{1}---",code,state));
            //jsApiPay.GetOpenidAndAccessTokenFromCode(code);
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                //var model = Iwx_interfaceService.GetModel(new KodyCRM.DomainModels.Query.Admin.wx_interfaceQuery());
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(WxPayConfig.APPID, WxPayConfig.APPSECRET, code);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---openid:出错---"));
                return Content("您拒绝了授权！");
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                //return Json(new Result(-2, "请求失效,请重新进入购票"));
                return Content("错误：" + result.errmsg);
            }
            try
            {
                var openid = result.openid;
                var user = IAccountService.GetByopenID(openid);
                if (user == null)
                {
                    OAuthUserInfo userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(result.access_token, result.openid);
                    AccountUser modelUser = new AccountUser();
                    modelUser.Address = "";
                    modelUser.DisplayName = userInfo.nickname;
                    modelUser.Email = "";
                    modelUser.Gender = userInfo.sex;
                    modelUser.Mobile = "";
                    modelUser.Name = userInfo.openid;
                    modelUser.Photo = userInfo.headimgurl.Replace("/0", "/132");
                    //modelUser.SetPassword(Password);
                    modelUser.State = UserStates.Normal;
                    MembershipService.CreateUser(modelUser);
                    Account modelAccount = new Account();
                    modelAccount.activatePoint = 0;
                    modelAccount.orangeKey = modelUser.UserId.ToString().PadLeft(modelUser.UserId.ToString().Length + 2, '0');
                    modelAccount.openID = userInfo.openid;
                    modelAccount.salerId = 0;
                    modelAccount.state = AccountStates.Normal;
                    modelAccount.submitTime = DateTime.Now;
                    modelAccount.userId = modelUser.UserId;
                    modelAccount.grade = AccountGrade.not;
                    IAccountService.Insert(modelAccount);
                    HttpCookie cookie = new HttpCookie(SessionKeys.USERID, modelUser.UserId.ToString());
                    Response.Cookies.Add(cookie);
                    Session[SessionKeys.USERID] = modelUser.UserId;
                }
                else
                {
                    var modelUser = MembershipService.GetUserById(user.userId);
                    HttpCookie cookie = new HttpCookie(SessionKeys.USERID, modelUser.UserId.ToString());
                    Response.Cookies.Add(cookie);
                    Session[SessionKeys.USERID] = modelUser.UserId;
                }
                return RedirectToAction("index", "PersonalCentre");
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), ex.Message+"||"+ex.StackTrace+"||"+ex.TargetSite);
                return Content("授权失败！");
            }
        }

        public ActionResult AppLogin(string mobile,string type)
        {
            if (type != "app")
            {
                return Json(new ResultMessage() { Code = -1, Msg = "无权访问!" });
            }
            if(string.IsNullOrWhiteSpace(mobile))
                return Json(new ResultMessage() { Code = -1, Msg = "请输入手机号!" });
            var user = IAccountService.GetByMobile(mobile);
            if (user == null)
            {
                user = new Account();
                user.gender = 0;
                user.name = "";
                user.openID = "";
                user.state = AccountStates.Normal;
                user.submitTime = DateTime.Now;
                user.mobile = mobile;
                if (IAccountService.Insert(user) > 0)
                {
                    Session["accountId"] = user.accountId;
                    return Json(new ResultMessage() { Code = 0, Msg = "index.html" });
                }
                return Json(new ResultMessage() { Code = -1, Msg = "注册用户失败!" });
            }
            else
            {
                Session["accountId"] = user.accountId;
                return Json(new ResultMessage() { Code = 0, Msg = "/WeChatViews/index.html" });
            }
        }

        public ActionResult OrderLogin(string code, string state)
        {
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---进入了订单登录回调---"));
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---code:{0},state:{1}---", code, state));
            //jsApiPay.GetOpenidAndAccessTokenFromCode(code);
            if (string.IsNullOrEmpty(code))
            {
                //return Json(new ResultMessage() { Code = -1, Msg = "参数错误，请联系管理员！" });
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                //var model = Iwx_interfaceService.GetModel(new KodyCRM.DomainModels.Query.Admin.wx_interfaceQuery());
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(WxPayConfig.APPID, WxPayConfig.APPSECRET, code);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---openid:出错---"));
                return Content("您拒绝了授权！");
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                //return Json(new Result(-2, "请求失效,请重新进入购票"));
                return Content("错误：" + result.errmsg);
            }
            try
            {
                var openid = result.openid;
                var user = IAccountService.GetByopenID(openid);
                if (user == null)
                {
                    return Content("用户信息不存在");
                }
                else
                {
                    OAuthUserInfo userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(result.access_token, result.openid);
                    Session["openid"] = openid;
                    Session["accountId"] = user.accountId;
                    user.photo = userInfo.headimgurl.Replace("/0", "/132");
                    user.name = userInfo.nickname;
                    IAccountService.Update(user);
                    if (string.IsNullOrWhiteSpace(user.mobile))
                    {
                        //未绑定手机号，跳转绑定手机号
                        return Redirect(string.Format("/WeChatViews/i-banding.html?openid={0}&name={1}&photo={2}", openid, System.Web.HttpUtility.UrlEncode(userInfo.nickname), userInfo.headimgurl.Replace("/0", "/132")));
                    }
                    //if (state == "1")
                    //    return Redirect("/WeChatViews/index.html?id=" + user.accountId);
                    //else if (state == "2")
                    //    return Redirect("/WeChatViews/i.html?id=" + user.accountId);
                    //else if (state == "3")
                    //    return Redirect("/WeChatViews/i.html?id=" + user.accountId);
                    //else if (state == "4")
                    //    return Redirect("/WeChatViews/i.html?id=" + user.accountId);
                    //else if (state == "5")
                    //{
                    //    return Redirect("/WeChatViews/i.html?id=" + user.accountId);
                    //}
                    //else
                    return Redirect("/WeChatViews/ddxq.html?orderNo=" + state);

                }
                return Content("授权失败！");
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), ex.Message + "||" + ex.StackTrace + "||" + ex.TargetSite);
                return Content("授权失败！");
            }
        }

        //public ActionResult 

        public ActionResult Register(string code, string state)
        {
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---进入了用户信息回调(注册)---"));
            WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---code:{0},state:{1}---", code, state));
            //jsApiPay.GetOpenidAndAccessTokenFromCode(code);
            if (string.IsNullOrEmpty(code))
            {
                //return Json(new ResultMessage() { Code = -1, Msg = "参数错误，请联系管理员！" });
                return Content("您拒绝了授权！");
            }
            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                //var model = Iwx_interfaceService.GetModel(new KodyCRM.DomainModels.Query.Admin.wx_interfaceQuery());
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(WxPayConfig.APPID, WxPayConfig.APPSECRET, code);
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---openid:出错---"));
                return Content("您拒绝了授权！");
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                //return Json(new Result(-2, "请求失效,请重新进入购票"));
                return Content("错误：" + result.errmsg);
            }
            
            var openid = result.openid;
            var user = IAccountService.GetByopenID(openid);
            if (user == null)
            {
                int salerId = 0;
                int.TryParse(state, out salerId);
                OAuthUserInfo userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(result.access_token, result.openid);
                user = new Account();
                user.salerId = salerId;
                user.gender = userInfo.sex;
                user.name = userInfo.nickname;
                user.openID = openid;
                user.state = AccountStates.Normal;
                user.submitTime = DateTime.Now;
                //WxPayAPI.Log.Info(this.GetType().ToString(), string.Format("---头像:{0}---", userInfo.headimgurl));
                user.photo = userInfo.headimgurl.Replace("/0", "/132");
                if (IAccountService.Insert(user) > 0)
                {
                    Session["openid"] = openid;
                    Session["accountId"] = user.accountId;
                    return Redirect("/WeChatViews/JuMeiMallIndex.html?id=" + user.accountId);
                }
            }
            else
            {
                return Redirect("/WeChatViews/JuMeiMallIndex.html?id=" + user.accountId);

            }
            return Content("授权失败！");
        }

        public ActionResult LoginOut()
        {
            if (Request.Cookies.AllKeys.Contains(SessionKeys.USERID))
            {
                var cookie = Request.Cookies[SessionKeys.USERID];
                cookie.Expires = DateTime.Now.AddDays(-3);
                Response.Cookies.Add(cookie);
            }
            Session[SessionKeys.USERID] = null;
            return RedirectToAction("Index", "Login");
        }

        
    }
}
