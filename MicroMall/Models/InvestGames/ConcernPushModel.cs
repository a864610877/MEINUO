using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.InvestGames
{
    public class ConcernPushModel
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID） 
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型） 
        /// </summary>
        public int CreateTime { get; set; }
        /// <summary>
        /// 消息类型，event 
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅) 
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值 
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}