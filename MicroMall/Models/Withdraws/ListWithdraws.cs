using Ecard;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Withdraws
{
    public class ListWithdraws : layouts.LayoutModel
    {
        [Dependency, NoRender]
        public IWithdrawService WithdrawService { get; set; }

        public ListWithdraw item { get; set; }

        public ListWithdraw Query(int pageIndex)
        {
            var user = _securityHelper.GetCurrentUser();
            if (user != null)
            {
                var request = new WithdrawRequest();
                request.PageIndex = pageIndex;
                request.UserId = user.CurrentUser.UserId;
                var query = WithdrawService.Query(request);
                if (query != null)
                {
                    var listWithdraw = new ListWithdraw();
                    listWithdraw.List = query.ModelList.Select(x => new Withdraw() { point = x.point, orderNo = x.orderNo, state = x.state, remark = x.remark, submitTime = x.submitTime }).ToList();
                    Page(request.PageIndex, request.PageSize, query.TotalCount, listWithdraw);
                    item = listWithdraw;
                    return listWithdraw;
                }
            }
            return null;
        }

        public void Page(int pageIndex, int pageSize, int total, ListWithdraw item)
        {
            item.PageIndex = pageIndex;
            int pageToTal = total / pageSize;
            int more = total % pageSize;
            if (more > 0)
                pageToTal += 1;
            if (pageIndex == pageToTal)
            {
                item.NextPage = 0;
                int prve = pageIndex - 1;
                if (prve <= 0)
                    item.PrevPage = 0;
                else
                {
                    item.PrevPage = prve;
                }
            }
            else if (pageIndex < pageToTal)
            {
                item.NextPage = pageIndex + 1;
                int prve = pageIndex - 1;
                if (prve <= 0)
                    item.PrevPage = 0;
                else
                {
                    item.PrevPage = prve;
                }
            }
            else
            {
                item.PrevPage = 0;
                item.NextPage = 0;
            }
        }
    }
}