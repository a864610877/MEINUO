using System;
using System.Collections.Generic;
using System.Reflection;
using Ecard.Models;
using Ecard.Services;
using Moonlit;
using log4net;

namespace Ecard.Mvc
{
    public class LogHelper
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogService _logService;
        private readonly I18NManager _i18NManager;
        private static Dictionary<int, string> LogTypes = new Dictionary<int, string>();
        static LogHelper()
        {
            var fields = typeof(LogTypes).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var fieldInfo in fields)
            {
                LogTypes.Add((int)fieldInfo.GetValue(null), fieldInfo.Name);
            }
        }
        public LogHelper(SecurityHelper securityHelper, ILogService logService, I18NManager i18NManager)
        {
            _securityHelper = securityHelper;
            _logService = logService;
            _i18NManager = i18NManager;
        }
        public void LogWithSerialNo(int logType, string serialNo, int addIn, params object[] args)
        {
            var log = GetLogWithSerialNo(logType, serialNo, addIn, args);
            _logService.Create(log);
        }

        public class LogItem
        {
            [Bounded(typeof(LogTypes))]
            public int LogType { get; set; }

            public int UserId { get; set; }
            public string Title { get; set; }
        }
        private static ILog Logger = LogManager.GetLogger("ecard.error");
        public void Error(int logType, Exception ex)
        {
            LogItem item = new LogItem() { LogType = logType };
            item.Title = ModelHelper.GetBoundText(item, x => x.LogType);
            var user = _securityHelper.GetCurrentUser();
            if(user != null)
            {
                item.UserId = user.CurrentUser.UserId;
            }
            Logger.Error(item, ex);
        }
        public void Error(int logType, string ex)
        {
            Error(logType, new Exception(ex));
        }
        public void Error(string title, Exception ex)
        {
            LogItem item = new LogItem() ;
            item.Title = title;
            var user = _securityHelper.GetCurrentUser();
            if (user != null)
            {
                item.UserId = user.CurrentUser.UserId;
            }
            Logger.Error(item, ex);
        }

        public Log GetLogWithSerialNo(int logType, string serialNo, int addIn, object[] args)
        {
            if (!LogTypes.ContainsKey(logType))
                throw new Exception("not support log type" + logType);
            var key = LogTypes[logType];
            var content = _i18NManager.Get("Ecard.Models.LogTypes", key + ".content", key + ".content");
            content = string.Format(content, args);
            var log = new Log(_i18NManager.Get("Ecard.Models.LogTypes", key + ".name", key + ".name"), content, logType,
                              _securityHelper.GetCurrentUser()) { AddIn = addIn, SerialNo = serialNo };
            return log;
        }
    }
}