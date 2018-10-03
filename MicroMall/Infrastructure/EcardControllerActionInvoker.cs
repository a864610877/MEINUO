 
using System;
using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.ViewModels;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.Results;

namespace Ecard.Mvc.Infrastructure
{
    public class EcardControllerActionInvoker : ControllerActionInvoker
    {
        private readonly IActionFilterRegistry registry;
        private readonly LogHelper Logger;

        public EcardControllerActionInvoker(IActionFilterRegistry registry,  LogHelper logger)
        {
            this.registry = registry;
            Logger = logger;
        }
         
        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, System.Collections.Generic.IDictionary<string, object> parameters)
        {
            try
            {
                using (new RunWatcher("执行方法: " + controllerContext.Controller.GetType().Name + " -- " + actionDescriptor.ActionName))
                {
                    //using (_databaseInstance)
                    {
                        var result = base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
                        return result;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Logger.Error(actionDescriptor.ActionName, ex);
                return new ErrorResult(string.Format("对不起，系统访问错误，您是否有参数没有正确填入！<br /> 出错时间:{0}", DateTime.Now.ToLongTimeString()));
            }
            catch (Exception ex)
            { 
                Logger.Error(actionDescriptor.ActionName, ex);
                return new ErrorResult(string.Format("对不起，系统发生内部错误，请重试！<br />{0}<br /> 出错时间:{1}", ex.Message, DateTime.Now.ToLongTimeString()));
            }
        }
        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            try
            {
                // 如果结果没有数据，则返回空
                if (actionReturnValue == null)
                {
                    controllerContext.Controller.ViewData.Model = new EcardModel { Container = new NotFoundPageContainer() };

                    return new NotFoundResult();
                }
                // 如果结果是 ActionResult， 直接返回
                if (typeof(ActionResult).IsAssignableFrom(actionReturnValue.GetType()))
                    return actionReturnValue as ActionResult;

                // 否则直接将结果赋给 Model
                controllerContext.Controller.ViewData.Model = actionReturnValue;
                return new ViewResult { ViewData = controllerContext.Controller.ViewData, TempData = controllerContext.Controller.TempData };
            }
            catch (Exception ex)
            {
                Logger.Error(actionDescriptor.ActionName, ex); 
                return new ErrorResult("对不起，系统发生内部错误，请重试！<br /> 出错时间:" + DateTime.Now.ToLongTimeString());
            }
        }

        /// <summary>
        /// 从 <see cref="registry"/> 中获取合适的 Filters 填充到 ActionFilters 中
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            FilterInfo baseFilters = base.GetFilters(controllerContext, actionDescriptor);
            FilterInfo registeredFilters = registry.GetFilters(new ActionFilterRegistryContext(controllerContext, actionDescriptor));

            foreach (IActionFilter actionFilter in registeredFilters.ActionFilters)
                baseFilters.ActionFilters.Insert(0, actionFilter);
            foreach (IAuthorizationFilter authorizationFilter in registeredFilters.AuthorizationFilters)
                baseFilters.AuthorizationFilters.Add(authorizationFilter);
            foreach (IExceptionFilter exceptionFilter in registeredFilters.ExceptionFilters)
                baseFilters.ExceptionFilters.Add(exceptionFilter);
            foreach (IResultFilter resultFilter in registeredFilters.ResultFilters)
                baseFilters.ResultFilters.Add(resultFilter);

            return baseFilters;
        }
    }
}
