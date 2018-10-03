using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.PersonalCentre
{
    public class ListTeams : ResultMessage
    {
        public List<ListTeam> List { get; set; }
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

        public IAccountService IAccountService { get; set; }

        public ListTeams(IAccountService IAccountService)
        {
            this.IAccountService = IAccountService;
        }

        public void Query(AccountSaleRequest request)
        {
            var query = IAccountService.GetSaleTeam(request);
            if (query != null&&query.ModelList!=null)
            {
                List = query.ModelList.Select(x => new ListTeam()
                {
                    accountId = x.accountId,
                    gender = x.gender == 1 ? "男" : "女",
                    grade = x.grade,
                    name = x.name,
                    photo = x.photo
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

    public class ListTeam
    {
        public int accountId { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string grade { get; set; }
    }
}