using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Reviews
{
    public class ListReviews : EcardModelListRequest<ListReview>
    {
        public ListReviews() 
        {
            OrderBy = "submitTime Desc";
        }
        [Dependency]
        [NoRender]
        public IReviewService ReviewService { get; set; }

        public string UserName { get; set; }

        public string CommodityNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        private Bounded _state;
        public Bounded State
        {
            get
            {
                if (_state == null)
                    _state = Bounded.Create<Reviewss>("State", ReviewStates.StayCheck);
                return _state;
            }
            set { _state = value; }
        }

        public void Query()
        {
            var request = new ReviewRequest();
            request.State = ReviewStates.StayCheck;
            var query = ReviewService.Query(request);
            if(query!=null)
            {
                List = query.ModelList.Select(x => new ListReview(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListReview>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            
        }

        public List<ListReview> AjaxQuery(ReviewRequest request)
        {
            if (request.State == ReviewStates.All)
                request.State = null;
            var query = ReviewService.Query(request);
            if (query != null)
            {
                var list = query.ModelList.Select(x => new ListReview(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
                return list;
            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
                return new List<ListReview>();
            }
        }


       
    }
}
