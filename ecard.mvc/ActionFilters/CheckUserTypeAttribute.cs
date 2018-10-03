using System;
using System.Linq;
using System.Web.Mvc;
using Ecard.Models;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CheckUserTypeAttribute : ActionFilterAttribute, IPermissionAttribute
    {
        private readonly Type[] _userTypes;

        public CheckUserTypeAttribute(params Type[] userTypes)
        {
            _userTypes = userTypes;
        }
         
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var helper = EcardContext.Container.Resolve<SecurityHelper>();
                var user = helper.GetCurrentUser();
                var canAccess = Check(user.CurrentUser);
                if (!canAccess)
                {
                    filterContext.Result = new ErrorResult("您没有权限进行此操作");
                }
            }
            base.OnActionExecuting(filterContext);
        }


        public bool Check(User user)
        {
            return _userTypes.Any(x => x.IsAssignableFrom(user.GetType())); 
        }
    }
}