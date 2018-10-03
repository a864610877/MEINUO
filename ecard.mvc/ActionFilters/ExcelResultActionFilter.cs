using System.Web.Mvc;
using Ecard.Models;
using Ecard.Mvc.Results;
using Ecard.Mvc.ViewModels;
using Oxite.Mvc.Results;

namespace Ecard.Mvc.ActionFilters
{
    public class ExcelResultActionFilter : IActionFilter
    {
        private readonly SecurityHelper _securityHelper;

        public ExcelResultActionFilter(SecurityHelper securityHelper)
        {
            _securityHelper = securityHelper;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            object model = filterContext.Controller.ViewData.Model;
            IItemList itemlist = model as IItemList;

            if (itemlist != null)
            {
                IPageOfList pagedList = itemlist.Items as IPageOfList;
                if (pagedList != null)
                {
                    pagedList.PageIndex = 1;
                    pagedList.PageSize = int.MaxValue;
                    filterContext.Result = new ExcelResult(itemlist.Items, pagedList.GetType().GetGenericArguments()[0], _securityHelper);
                }
            }
            EcardModelReportRequest report = model as EcardModelReportRequest;
            if (report != null)
            {
                report.PageIndex = 1;
                report.PageSize = int.MaxValue;
                filterContext.Result = new ExcelTableResult(report.Table);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var parameterName in filterContext.ActionParameters.Keys)
            {
                var parameter = filterContext.ActionParameters[parameterName] as IQueryRequest;
                if (parameter != null)
                {
                    parameter.PageIndex = 1;
                    parameter.PageSize = int.MaxValue;
                }
                EcardModelReportRequest report = filterContext.ActionParameters[parameterName] as EcardModelReportRequest;
                if (report != null)
                {

                    parameter.PageIndex = 1;
                    parameter.PageSize = int.MaxValue;
                }
            }
        }
    }
}