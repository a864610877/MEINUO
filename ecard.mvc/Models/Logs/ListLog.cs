using System;
using Ecard.Models;

namespace Ecard.Mvc.Models.Logs
{
    public class ListLog
    {
        private readonly Log _innerObject;

        [NoRender]
        public Log InnerObject
        {
            get { return _innerObject; }
        }

        public ListLog()
        {
            _innerObject = new Log();
        }
        
        public ListLog(Log innerObject)
        {
            _innerObject = innerObject;
        }

        public DateTime SubmitTime
        {
            get { return InnerObject.SubmitTime; }
        }

        public string UserName
        {
            get { return InnerObject.UserName; }
        } 
        public string Title
        {
            get { return InnerObject.Title; }
        }

        public string Content
        {
            get { return InnerObject.Content; }
        }
        public string SerialNo
        {
            get { return InnerObject.SerialNo; }
        }
        [NoRender]
        public int LogId
        {
            get { return InnerObject.LogId; }
        }
         
    }
}