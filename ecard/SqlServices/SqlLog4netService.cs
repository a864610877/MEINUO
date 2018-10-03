using Ecard.Services;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Ecard.SqlServices
{
    public class SqlLog4netService : ILog4netService
    {
        public string url;
        private readonly ILog _log;
        public SqlLog4netService()
        {
            string config = HttpContext.Current.Server.MapPath("~/log4net.config");   //AppDomain.CurrentDomain.BaseDirectory + @"\log4net.config";           
            var repository = new FileInfo(config);
           // XmlConfigurator.Configure(repository);
            //logService = new LogService();
            _log = log4net.LogManager.GetLogger(typeof(SqlLog4netService));
        }
        public void Insert(Exception ex)
        {
            _log.Debug(ex);
        }

        public void Insert(string msg)
        {
            _log.Debug(msg);
        }
    }
}
