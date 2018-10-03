using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.OperationPointLogs
{
    public class OperationPointLogLists : EcardModelListRequest<OperationPointLogList>
    {
        public OperationPointLogLists() 
        {
            OrderBy = "submitTime Desc";
        }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
        [Dependency]
        [NoRender]
        public IOperationPointLogService operationPointLogService { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(OperationPointLogList item)
        {
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.operationPointLogId });
        }

        public void Query()
        {
            var request = new OperationPointLogRequest();
            var query = operationPointLogService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new OperationPointLogList(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<OperationPointLogList>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }
        public List<OperationPointLogList> AjaxQuery(OperationPointLogRequest request)
        {

            var data = new List<OperationPointLogList>();
            var query = operationPointLogService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new OperationPointLogList(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Delete','/RecommendLog/Delete/" + item.operationPointLogId + "') class='tablelink'>删除 </a> ";
                }
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);

            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            return data;
        }
    }
}
