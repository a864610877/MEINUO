using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.InvestGames
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public class ReqEventTypes
    {
        /// <summary>
        /// subscribe(订阅)
        /// </summary>
        public const string subscribe = "subscribe";
        /// <summary>
        /// 取消订阅
        /// </summary>
        public const string unsubscribe = "unsubscribe";
        /// <summary>
        /// 单击自定义菜单事件
        /// </summary>
        public const string CLICK = "CLICK";

        /// <summary>
        /// 用户已关注时的事件推送
        /// </summary>
        public const string SCAN = "SCAN";
    }
}