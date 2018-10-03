using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Accounts
{
    public class ListAccounts : EcardModelListRequest<ListAccount>
    {
        public ListAccounts() 
        {
            OrderBy = "submitTime Desc";
        }

        public string DisplayName { get; set; }
        public string Mobile { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        [Dependency]
        [NoRender]
        public IAccountService accountService { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListAccount item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.accountId });
            //yield return new ActionMethodDescriptor("Delete", null, new { id = item.accountId });
            //yield return new ActionMethodDescriptor("GetSaleAmount", null, new { id = item.accountId });
        }

        public void Query() 
        {
            var request = new AccountRequest();
            var query = accountService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListAccount(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListAccount>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<ListAccount> AjaxQuery(AccountRequest request)
        {

            var data = new List<ListAccount>();
            var query = accountService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListAccount(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Edit','/Account/Edit/" + item.accountId + "') class='tablelink'>编辑 &nbsp;</a> ";
                   // item.boor += "<a href='#' onclick=OperatorThis('Delete','/Account/Delete/" + item.accountId + "') class='tablelink'>删除 </a> ";
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
