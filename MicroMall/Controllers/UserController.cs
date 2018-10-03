using Ecard.Models;
using Ecard.Mvc;
using Ecard.Services;
using MicroMall.Models;
using MicroMall.Models.Homes;
using MicroMall.Models.Users;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MicroMall.Controllers
{
    public class UserController : Controller
    {

        private readonly IUnityContainer _unityContainer;
        private readonly Site _currentSite;
        private readonly SecurityHelper _securityHelper;
        public IMembershipService MembershipService { get; set; }
        public UserController(IMembershipService membershipService, IUnityContainer unityContainer, Site currentSite, SecurityHelper securityHelper)
        {
            _unityContainer = unityContainer;
            _currentSite = currentSite;
            _securityHelper = securityHelper;
            FormsService = new FormsAuthenticationService();
            MembershipService = membershipService;
          
        }
        public IFormsAuthenticationService FormsService { get; set; }
        //
        // GET: /User/
        [HttpPost]
        public ActionResult Login(string signinUserName, string signinPassword, string signinCode, string logonToken)
        {
            var curCode = UnityController.GetCode(this.HttpContext, "shopsignin");
            if (curCode != signinCode)
            {
                ViewData["msg"] = "验证码错误";
                //return Json(new SimpleAjaxResult("验证码错误"));
                return Login();
            }
            var tokenOnServer = LogonToken;
            LogonToken = Guid.NewGuid().ToString("N");
            if (_unityContainer.Resolve<IAuthenticateService>("password").ValidateUser(signinUserName, signinPassword, logonToken, tokenOnServer))
            {
                FormsService.SignIn(signinUserName, false);
                Session.Clear();
                //return Json(new SimpleAjaxResult());
                return RedirectToAction("Index", "Home");
            }
            ViewData["msg"] = "用户名或密码错";
            return Login();
        }
        [Authorize]
        public ActionResult LogOff()
        {
            try
            {
                var context = System.Web.HttpContext.Current;
                var key = "__currentUser_" + context.User.Identity.Name;
                context.Cache.Remove(key);
            }
            catch
            { }
            FormsService.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        protected string LogonToken
        {
            get { return (string)Session["user_controller_logon_LogonToken"]; }
            set { Session["user_controller_logon_LogonToken"] = value; }
        }
        public ActionResult Login()
        {
            var request = _unityContainer.Resolve<LoadIndex>();
            //ViewData["msg"] = "";
            return View(request);
        }

        public ActionResult UserInformation(EditUser request)
        {
            //var request = _unityContainer.Resolve<EditUser>();
            request.Ready();
            return View(request);
        }

        public ActionResult EditInformation()
        {
            var request = _unityContainer.Resolve<EditUser>();
            request.Ready();
            request.GetProvince();
            return View(request);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditInformation(EditUser request)
        {
            return Json(request.Save());
        }

        public ActionResult PromotionLog(ListPromotionLogs request)
        {
            //request.Load();
            //request.reayd();
            //request.Query();
            return View(request);
        }

        public ActionResult RecommendLog(int PageIndex=1)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
           int userId = 0;
           int.TryParse(strId, out userId);
           var request = _unityContainer.Resolve<ListPromotionLogs>();
           request.reayd();
           request.Query(userId);
           //var result = request.AjaxRecommendLog(PageIndex, userId);
           return View(request);
        }

        [HttpPost]
        public ActionResult AjaxRecommendLog(int PageIndex)
        {
            if (Request.Cookies[SessionKeys.USERID] == null || Request.Cookies[SessionKeys.USERID].Value.ToString() == "")
            {
                return RedirectToAction("Index", "login");
                //return Json(new ResultMessage() { Code = -2, Msg = "/login/Index" });
            }
            var strId = Request.Cookies[SessionKeys.USERID].Value.ToString();
           int userId = 0;
           int.TryParse(strId, out userId);
            var request = _unityContainer.Resolve<ListPromotionLogs>();
            request.reayd();
            return Json(request.AjaxRecommendLog(PageIndex,userId));
        }
        [HttpPost]
        public ActionResult AjaxPointLog(int PageIndex)
        {
            var request = _unityContainer.Resolve<ListPromotionLogs>();
            request.reayd();
            return Json(request.AjaxPointLog(PageIndex));
        }


      

    }
    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable. 

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion
}
