using Ecard.Infrastructure;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Articles
{
    public class ListArticles : EcardModelListRequest<ListArticle>
    {
        [Dependency]
        [NoRender]
        public IArticlesService IArticlesService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }
        public ListArticles()
        {
            OrderBy = "submitTime desc";
        }

        public string title { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListArticle item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.articleId });
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.articleId });
        }
        public void Query()
        {
            var request = new ArticlesRequest();
            var query = IArticlesService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListArticle(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListArticle>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }
        public List<ListArticle> AjaxQuery(ArticlesRequest request)
        {

            var data = new List<ListArticle>();
            var query = IArticlesService.Query(request);
            if (query != null)
            {
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                data = query.ModelList.Select(x => new ListArticle(x)).ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("EditArticles"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/Articles/Edit/" + item.articleId + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("DeleteArticles"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Delete','/Articles/Delete/" + item.articleId + "') class='tablelink'>删除 </a> ";
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

    public class ListArticle
    { 
        private readonly Ecard.Models.Articles _innerObject;
        [NoRender]
        public Ecard.Models.Articles InnerObject
        {
            get { return _innerObject; }
        }

        public ListArticle()
        {
            _innerObject = new Ecard.Models.Articles();
        }

        public ListArticle(Ecard.Models.Articles innerObject)
        {
            _innerObject = innerObject;
        }
        [NoRender]
        public int articleId { get { return InnerObject.articleId; } }

        public int articleId1 { get { return InnerObject.articleId; } }
        public string title { get { return InnerObject.title; } }
        public string submitTime { get { return InnerObject.submitTime.ToString("yyyy-MM-dd"); } }

        [NoRender]
        public string boor { get; set; }
    }
}
