using Ecard.Models;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Users
{
    public class ListPromotionLogs : LayoutModel
    {
        [Dependency]
        public IRecommendLogService RecommendLogService { get; set; }
        [Dependency]
        public IOperationPointLogService OperationPointLogService { get; set; }
         [Dependency]
        public IOperationAmountLogsService OperationAmountLogsService { get; set; }
        [Dependency]
         public IAccountService AccountService { get; set; }
        [Dependency]
        public IWithdrawService WithdrawService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Dependency]
        public IRebateLogService RebateLogService { get; set; }

        public ListPromotionLog RecommendLogs { get; set; }

        public ListPromotionLog PointLogs { get; set; }

        public string grade { get; set; }

        public decimal presentExp { get; set; }

        /// <summary>
        /// 可用于提）
        /// </summary>
        public decimal activatePoint { get; set; }
        /// <summary>
        /// 提现积分汇总
        /// </summary>
        public decimal withdrawPoint { get; set; }

        public void reayd()
        {
            RecommendLogs = new ListPromotionLog();
            PointLogs = new ListPromotionLog();
        }
        public void Query(int userId)
        {
            //var request = new MemberRecommendLogRequest();
            //request.salerId = userId;
            //request.PageSize = 1000;
            var account = AccountService.GetByUserId(userId);
            var query = RecommendLogService.GetList(account.accountId);
           if(query!=null)
           {
               RecommendLogs.List = query.Select(x => new PromotionModel() {  Name = x.DisplayName,value=x.grade.ToString(),tj=x.tj }).ToList();
               //Page(request.PageIndex, request.PageSize, query.TotalCount, RecommendLogs);
           }
          
           presentExp = account.presentExp;
           activatePoint = account.activatePoint;
           grade = AccountGrade.GetName(account.grade);
           withdrawPoint = WithdrawService.GetUserIdPoint(account.userId);
           var query1 = RebateLogService.GetRebateLog(account.accountId);
           if(query1!=null)
           {
                PointLogs.List = query1.Select(x => new PromotionModel() {
                    value = x.reateAmount.ToString(), submitTime = x.submitTime.ToString(),
                    Name = string.Format("来自{0}的会员佣金", x.DisplayName),
                    type = x.type == RebateType.zero ? "" : x.type == RebateType.one ? "会员分享奖励" : x.type == RebateType.two ? "店长分享奖励" : x.type == RebateType.three ? "店主分享奖励" : ""
               }).ToList();
              // Page(request1.PageIndex, request1.PageSize, query1.TotalCount, PointLogs);
           }
        }

        public ListPromotionLog AjaxRecommendLog(int PageIndex,int userId)
        {
           var request = new MemberRecommendLogRequest();
           request.PageIndex = PageIndex;
           request.PageSize = 1000;
           request.salerId = userId;
           var query = RecommendLogService.MemberQuery(request);
           if (query != null)
           {
               RecommendLogs.List = query.ModelList.Select(x => new PromotionModel() { submitTime = x.submitTime.ToString(), Name = string.Format("推荐会员{0}成功", x.userName) }).ToList();
               Page(request.PageIndex, request.PageSize, query.TotalCount, RecommendLogs);
           }
            return RecommendLogs;
        }

        public ListPromotionLog AjaxPointLog(int pageIndex)
        {
            Load();
            if (UserInformation != null)
            {
                var request1 = new OperationPointLogRequest();
                request1.PageIndex = pageIndex;
                request1.userId = UserInformation.UserId;
                var query1 = OperationPointLogService.Query(request1);
                if (query1 != null)
                {
                    PointLogs.List = query1.ModelList.Select(x => new PromotionModel() { submitTime = x.submitTime.ToString(), Name = string.Format("{0}{1}", x.remark, x.point) }).ToList();
                    Page(request1.PageIndex, request1.PageSize, query1.TotalCount, PointLogs);
                }
            }
            return PointLogs;
        }

        public void Page(int pageIndex, int pageSize, int total, ListPromotionLog item)
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