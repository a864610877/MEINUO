using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.InvestGames
{
    public class ConcernPush : ConcernPushModel
    {
        [Dependency]
        public ILog4netService logService { get; set; }
        [Dependency]
        public IOrangeKeyAndopenIDService OrangeKeyAndopenIDService{get;set;}

        public string Save(string xml)
        {
            try
            {
                logService.Insert(string.Format("开始推送关注事件！"));
                string param = string.Format("ToUserName:{0},FromUserName:{1},CreateTime:{2},MsgType:{3},Event:{4},EventKey:{5},Ticket{6}", ToUserName, FromUserName, CreateTime, MsgType, Event, EventKey, Ticket);
                logService.Insert(param);
                string messaageId = FromUserName + CreateTime;
                if (string.IsNullOrWhiteSpace(messaageId))
                {
                    logService.Insert(string.Format("参数错误！"));
                    return "";
                }
                var old = OrangeKeyAndopenIDService.GetBymessageId(messaageId);
                if (old != null)
                {
                    logService.Insert(string.Format("old！"));
                    return "";
                }
                   
                if (Event == ReqEventTypes.subscribe)
                {
                    if (!string.IsNullOrWhiteSpace(EventKey))
                    {
                        var item = OrangeKeyAndopenIDService.GetByopenID(FromUserName);
                        if (item == null)
                        {
                            OrangeKeyAndopenID model = new OrangeKeyAndopenID();
                            model.messageId = messaageId;
                            model.openID = FromUserName;
                            model.orangeKey = EventKey.Replace("qrscene_", "");
                            model.submitTime = DateTime.Now;
                            OrangeKeyAndopenIDService.Insert(model);
                        }
                    }
                    logService.Insert(string.Format("{0}关注成功！", FromUserName));
                }
                else
                {
                    logService.Insert(string.Format("{0}取消订阅", FromUserName));
                }
                return "";
            }
            catch(Exception ex)
            {
                logService.Insert(ex);
                return "";
            }
        }

     
    }
}