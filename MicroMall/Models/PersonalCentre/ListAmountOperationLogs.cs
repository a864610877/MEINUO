using Ecard;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class ListAmountOperationLogs: ResultMessage
    {
        public List<ListAmountOperationLog> List { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }
        [Dependency, NoRender]
        public IOperationAmountLogsService IOperationAmountLogsService { get; set; }

        public void Query(OperationAmountLogUserIdRequest request)
        {
            var query = IOperationAmountLogsService.GetByUserId(request);
            if (query != null && query.ModelList != null)
            {
                List = query.ModelList.Select(x => new ListAmountOperationLog()
                {
                  category = OperationAmountCategory.GetByName(x.category),
                  perationAmountLogId=x.perationAmountLogId,
                  submitTime=x.submitTime.ToString(),
                  type=x.type,
                  amount=x.amount
                }).ToList();
                PageIndex = request.PageIndex;
                int TotalPage = Math.Max((query.TotalCount + request.PageSize - 1) / request.PageSize, 1);
                if (request.PageIndex == TotalPage)
                {
                    NextPage = 0;
                    //PrePage = request.PageIndex - 1;
                }
                else if (request.PageIndex < TotalPage)
                {
                    NextPage = request.PageIndex + 1;
                    //response.PrePage = request.PageIndex - 1;
                }
            }
        }
    }

    public class ListAmountOperationLog
    {
        public int perationAmountLogId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string category { get; set; }

        public decimal amount { get; set; }
        ///// <summary>
        ///// 来源
        ///// </summary>
        //public string source { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string submitTime { get; set; }
    }
}