using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Grades
{
    public class ListGrades : EcardModelListRequest<ListGrade>
    {
        [Dependency]
        [NoRender]
        public IGradesService IGradesService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }
        public ListGrades()
        {
            OrderBy = "gradeId desc";
        }

        public string name { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListGrade item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.gradeId });
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.gradeId });
        }
        public void Query()
        {
            var request = new GradesRequest();
            var query = IGradesService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListGrade(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListGrade>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }
        public List<ListGrade> AjaxQuery(GradesRequest request)
        {

            var data = new List<ListGrade>();
            var query = IGradesService.Query(request);
            if (query != null)
            {
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                data = query.ModelList.Select(x => new ListGrade(x)).ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("EditGrades"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/Grades/Edit/" + item.gradeId + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("DeleteGrades"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Delete','/Grades/Delete/" + item.gradeId + "') class='tablelink'>删除 </a> ";
                    }
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

    public class ListGrade
    { 
      private readonly Ecard.Models.Grades _innerObject;
        [NoRender]
      public Ecard.Models.Grades InnerObject
        {
            get { return _innerObject; }
        }

        public ListGrade()
        {
            _innerObject = new Ecard.Models.Grades();
        }

        public ListGrade(Ecard.Models.Grades innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int gradeId { get { return InnerObject.gradeId; } }
        public string name { get { return InnerObject.name; } }
        public decimal sale { get { return InnerObject.sale; } }

        [NoRender]
        public string boor { get; set; }
    }
}
