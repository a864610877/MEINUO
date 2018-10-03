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

namespace Ecard.Mvc.Models.SecondKillCommodityss
{
    public class ListSecondKillCommodityss: EcardModelListRequest<ListSecondKillCommoditys>
    {
        public ListSecondKillCommodityss()
        {
            OrderBy = "id desc";
        }
        public string commodityNo { get; set; }
        [Dependency]
        [NoRender]
        public ISecondKillCommoditysService SecondKillCommoditysService { get; set; }
        [Dependency]
        [NoRender]
        public SecurityHelper _securityHelper { get; set; }

        public IEnumerable<ActionMethodDescriptor> GetToolbarActions()
        {
            yield return new ActionMethodDescriptor("Create");
            yield return new ActionMethodDescriptor("Deletes");
        }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListSecondKillCommoditys item)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = item.Id });
            yield return new ActionMethodDescriptor("Delete", null, new { id = item.Id });
        }

        public void Query()
        {
            var request = new SecondKillCommoditysRequest();
            request.commodityNo = commodityNo;
            var query = SecondKillCommoditysService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListSecondKillCommoditys(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListSecondKillCommoditys>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }

        public List<ListSecondKillCommoditys> AjaxQuery(SecondKillCommoditysRequest request)
        {

            var data = new List<ListSecondKillCommoditys>();
            var query = SecondKillCommoditysService.Query(request);
            if (query != null)
            {

                data = query.ModelList.Select(x => new ListSecondKillCommoditys(x)).ToList();
                var roles = _securityHelper.GetCurrentUser().CurrentUser.Roles.ToList();
                foreach (var item in data)
                {
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("ManageSecondKillCommoditys"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Edit','/SecondKillCommoditys/Edit/" + item.Id + "') class='tablelink'>编辑 &nbsp;</a> ";
                    }
                    if (roles[0].IsSuper || roles[0].Permissions.Contains("ManageSecondKillCommoditys"))
                    {
                        item.boor += "<a href='#' onclick=OperatorThis('Delete','/SecondKillCommoditys/Delete/" + item.Id + "') class='tablelink'>删除 </a> ";
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

    

        public ResultMsg Delete(int id)
        {
            ResultMsg msg = new ResultMsg();
            try
            {
                var item = SecondKillCommoditysService.GetById(id);
                if (item != null)
                {
                    SecondKillCommoditysService.Delete(item);
                    Logger.LogWithSerialNo(LogTypes.SecondKillCommoditysDelete, SerialNoHelper.Create(), item.id, item.commodityNo);
                    msg.Code = 1;
                    msg.CodeText = "删除用户 " + item.commodityNo + " 成功";
                }
                else
                {
                    msg.CodeText = "不好意思,没有找到商品";
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.CodeText = "不好意思,系统异常";
                Logger.Error("删除秒杀商品", ex);
                return msg;
            }
        }

    }
}
