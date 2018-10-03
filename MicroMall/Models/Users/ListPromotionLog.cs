using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Users
{
    public class ListPromotionLog
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public int PrevPage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public int NextPage { get; set; }

        public List<PromotionModel> List { get; set; }
    }

}