using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.Models.Logs
{
    public class LogModelBase 
    {
        private Log _innerObject;

        public LogModelBase()
        {
            _innerObject = new Log();
        }

        public LogModelBase(Log shop)
        {
            _innerObject = shop;
        }

        [NoRender]
        public Log InnerObject
        {
            get { return _innerObject; }
        }

        protected void SetInnerObject(Log item)
        {
            _innerObject = item;
        }


        [Dependency]
        [NoRender]
        public ILogService LogService { get; set; }


    }
}