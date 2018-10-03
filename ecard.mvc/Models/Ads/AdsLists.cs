using Ecard.Infrastructure;
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

namespace Ecard.Mvc.Models.Ads
{
    public class AdsLists : EcardModelListRequest<AdsList>
    {

        public AdsLists()
        {
            OrderBy = "SubmitTime desc";
        }

        public string title { get; set; }
        private Bounded _state;

        public Bounded state
        {
            get
            {
                if (_state == null)
                    _state = Bounded.Create<Ad>("state", AdStates.all);
                return _state;
            }
            set { _state = value; }
        }
        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        [Dependency]
        [NoRender]
        public IAdService adService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(AdsList item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.adId });
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.adId });
        }

        public void Query()
        {
            var request = new AdRequest();
            var query = adService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new AdsList(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<AdsList>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<AdsList> AjaxQuery(AdRequest request)
        {

            var data = new List<AdsList>();
            var query = adService.Query(request);
            if (query != null)
            {

                data = query.ModelList.Select(x => new AdsList(x)).ToList();
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("AdsEdit"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/Ads/Edit/" + item.adId + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("AdsDelete"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Delete','/Ads/Delete/" + item.adId + "') class='tablelink'>删除 </a> ";
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
}
