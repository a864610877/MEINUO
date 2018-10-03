using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Withdraws
{
    public class WithdrawDtl
    {
        /// <summary>
        /// 金额
        /// </summary>
        public string amount { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }
    }

    public class WithdrawDtls
    {
        public WithdrawDtls()
        {

            WithdrawDtlList = new List<WithdrawDtl>();

        }
        public int TotalCount { get; set; }
        public List<WithdrawDtl> WithdrawDtlList { get; set; }

    }
}