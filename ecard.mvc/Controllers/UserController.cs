using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models;
using Ecard.Mvc.Models.Users;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        private readonly Site _currentSite;
        private readonly SecurityHelper _securityHelper;

        public UserController(IMembershipService membershipService, IUnityContainer unityContainer, Site currentSite, SecurityHelper securityHelper)
        {
            _unityContainer = unityContainer;
            _currentSite = currentSite;
            _securityHelper = securityHelper;
            FormsService = new FormsAuthenticationService();
            MembershipService = membershipService;
        }

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected string LogonToken
        {
            get { return (string)Session["user_controller_logon_LogonToken"]; }
            set { Session["user_controller_logon_LogonToken"] = value; }
        }
        public ActionResult LogOn()
        {
            LogonToken = Guid.NewGuid().ToString("N");
            return View("LogOn", new LogOnModel() { LogonToken = LogonToken });
        }

        [HttpPost]
        public ActionResult LogOn(string signinUserName, string signinPassword, string signinCode, string logonToken, string returnUrl)
        {
           // string regcode = RegClass.GetCode();//code为注册码，开发调试时用RegClass.GetCode();获取开发机的注册码
            //string regcode = "6e297f3cced45dfbb92365f1eecf390";

            ////时间限制
            //if (DateTime.Now > Convert.ToDateTime("2014-08-01"))
            //{
            //    return Json(new DataAjaxResult("注册码已经过期") { Data1 = LogonToken });
            //    //return new ErrorResult(string.Format("注册码已经过期，必须输入正确的注册码才能使用系统。出错时间:{0}", DateTime.Now.ToLongTimeString()), "NeedReg");
            //}

            //using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            //{
            //    //rsa.FromXmlString(pubkey);
            //    RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsa);

            //    f.SetHashAlgorithm("SHA1");
            //    //注册码
            //    byte[] key = Convert.FromBase64String(@"WIAhL0xrAA6jL3uy/zLX9zvOc7pC7VODJPAHd9Z62JpF417H5ytHPdyHWAWeamLQ3XNZ8W5zuv9WjEwgY7UBRbUcqMoVby6d5ZwP3Bv7uyimsO0yrwjMnrVtH96dQ+FZDRzSIM1QNMKbw7rT8n/W8vOTBEobgZFD8zb/qYDlRHI=");//用户名

            //    SHA1Managed sha = new SHA1Managed();

            //    byte[] name = sha.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_currentSite.Regcode));//用户名
            //    if (!f.VerifySignature(name, key))
            //        return Json(new DataAjaxResult("必须输入正确的注册码才能使用系统") { Data1 = LogonToken });
            //} 
            var curCode = UtilityController.GetCode(this.HttpContext, "signin");
            if (curCode != signinCode)
            {
                ViewData["msg"] = "验证码错误";
                //return Json(new SimpleAjaxResult("验证码错误"));
                return LogOn();
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
            return LogOn();
        }

        //[HttpPost]
        //public ActionResult Register(RegisterAccountUser model, string code)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        var curCode = UtilityController.GetCode(this.HttpContext, "register");
        //        if (curCode != model.Code)
        //            return Json(new SimpleAjaxResult("验证码错误"), JsonRequestBehavior.AllowGet);
        //        return Json(model.Register(), JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new SimpleAjaxResult("请填写完整字段"), JsonRequestBehavior.AllowGet);
        //}

        // **************************************
        // URL: /editProfile
        // **************************************

        [Authorize]
        public ActionResult EditProfile()
        {
            EditProfile model = _unityContainer.Resolve<EditProfile>();
            model.Ready();
            return View(new EcardModelItem<EditProfile>(model));
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditProfile([Bind(Prefix = "item")] EditProfile model)
        {
            IMessageProvider msg = null;
            if (ModelState.IsValid)
            {
                msg = model.Save();
                model = _unityContainer.Resolve<EditProfile>();
            }
            model.Ready();
            return View(new EcardModelItem<EditProfile>(model, msg));
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

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

        public ActionResult StartRecovery(RecoveryUser model, string code)
        {
            var curCode = UtilityController.GetCode(this.HttpContext, "recovery");
            if (curCode != code)
                return Json(new SimpleAjaxResult("验证码错误"), JsonRequestBehavior.AllowGet);
            return Json(model.Save(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Recovery(string token)
        {
            var model = _unityContainer.Resolve<RecoveryUserPassword>();
            model.Token = token;
            model.Ready();
            return View("Recovery", new EcardModelItem<RecoveryUserPassword>(model));
        }
        [HttpPost]
        public ActionResult Recovery([Bind(Prefix = "Item")]RecoveryUserPassword model, string token)
        {
            if (ModelState.IsValid)
            {
                model.Token = token;
                if (model.Save())
                    return RedirectToAction("LogOn", "User");
            }
            model.Ready();
            return View("Recovery", new EcardModelItem<RecoveryUserPassword>(model));
        } 
        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        
        [CheckPermission(Permissions.CreateDog)]
        [Authorize]
        public ActionResult CreateDog(int id)
        {
            var model = _unityContainer.Resolve<CreateDog>();
            model.Read(id);
            model.Ready();
            return View("CreateDog", new EcardModelItem<CreateDog>(model));
        }
        [HttpPost]
        [CheckPermission(Permissions.CreateDog)]
        public ActionResult CreateDog(CreateDog model)
        {
            return Json(model.Create(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult UserInfo()
        {
            return PartialView("UserInfo", _securityHelper.GetCurrentUser());
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

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[]{
                new ModelClientValidationStringLengthRule(FormatErrorMessage(metadata.GetDisplayName()), _minCharacters, int.MaxValue)
            };
        }
    }
    #endregion
}
