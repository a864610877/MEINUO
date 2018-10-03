using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using System.Web.Mvc;

namespace Ecard.Mvc.Models.Logs
{
    public class ListLogs : EcardModelListRequest<ListLog>
    {
        public ListLogs()
        {
            OrderBy = "LogId desc";
        }

        private string _content;

        public string Content
        {
            get { return _content.TrimSafty(); }
            set { _content = value; }
        }
        [Dependency]
        [NoRender]
        public ILogService LogService { get; set; }

        public void Ready()
        {
            LogType.Bind(null, true);
        }

        public void Delete(int id)
        {
            var item = this.LogService.GetById(id);
            if (item != null)
                LogService.Delete(item);
        }


        private string _userName;

        public string UserName
        {
            get { return _userName.TrimSafty(); }
            set { _userName = value; }
        }
        public void Query(out string pageHtml)
        {
            pageHtml = string.Empty;
            var request = new LogRequest();
            if (request.PageIndex == null || request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == null || request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            if (this.LogType != Globals.All)
                request.LogType = LogType;

            request.ContentWith = Content; 
            request.UserName = UserName;
            var query = LogService.NewQuery(request);
            this.List = query.ModelList.ToList(this, x => new ListLog(x));
            if (query != null)
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
        }
        public List<ListLog> AjaxGet(LogRequest request, out string pageHtml)
        {
            pageHtml = string.Empty;
            if (request.PageIndex == null || request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == null || request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            var _tables = LogService.NewQuery(request);
            var datas = _tables.ModelList.Select(x => new ListLog(x)).ToList();
            
            if (_tables != null)
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, _tables.TotalCount);
            return datas;
        }

        private Bounded _logTypeBounded;

        public Bounded LogType
        {
            get
            {
                if (_logTypeBounded == null)
                {
                    _logTypeBounded = Bounded.Create<Log>("LogType", Globals.All);
                }
                return _logTypeBounded;
            }
            set { _logTypeBounded = value; }
        }
        public IEnumerable<ActionMethodDescriptor> GetToolbarActions()
        {
            yield break;
        }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListLog item)
        {
            yield return new ActionMethodDescriptor("View", null, new { id = item.LogId });
        }
        //private Bounded _state;
        //public Bounded State
        //{
        //    get
        //    {
        //        if (_state == null)
        //        {
        //            _state = Bounded.Create<Log>("State", UserStates.Normal);
        //        }
        //        return _state;
        //    }
        //    set { _state = value; }
        //}
    }
}
