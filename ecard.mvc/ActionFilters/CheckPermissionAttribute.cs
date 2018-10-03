using System;
using System.Linq;
using System.Web.Mvc;
using Ecard.Models; 

namespace Ecard.Mvc.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CheckPermissionAttribute : ActionFilterAttribute, IPermissionAttribute
    {
        private readonly string _permissions;

        public CheckPermissionAttribute(string permissions)
        {
            _permissions = permissions;
        }

        public string Permissions
        {
            get { return _permissions; }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var helper = (SecurityHelper)EcardContext.Container.Resolve(typeof(SecurityHelper), null);
            bool canExecute = Check(helper.GetCurrentUser().CurrentUser);
            if (!canExecute)
                filterContext.Result = new ErrorResult("您没有权限进行此项操作");
            base.OnActionExecuting(filterContext);
        }


        public bool Check(User user)
        {
            if (user == null) return false;
            var roles = user.GetRoles();
            return (roles != null && roles.AsQueryable().Any(x => x.HasPermissions(Permissions)));
        }
    }
}