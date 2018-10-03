using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.RecommendLogs
{
    public class RecommendLogLists : EcardModelListRequest<RecommendLogList>
    {
        public RecommendLogLists() 
        {
            OrderBy = "submitTime Desc";
        }

        public string salerName { get; set; }
        public string userName { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
        [Dependency]
        [NoRender]
        public IRecommendLogService recommendLogService { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(RecommendLogList item)
        {
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.recommendLogId });
        }

        public void Query()
        {
            var request = new MemberRecommendLogRequest();
            var query = recommendLogService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new RecommendLogList(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<RecommendLogList>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }
        public List<RecommendLogList> AjaxQuery(MemberRecommendLogRequest request)
        {

            var data = new List<RecommendLogList>();
            var query = recommendLogService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new RecommendLogList(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Delete','/RecommendLog/Delete/" + item.recommendLogId + "') class='tablelink'>删除 </a> ";
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
