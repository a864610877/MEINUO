using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using log4net;

namespace Oxite.Infrastructure
{
    public class RunWatcher : IDisposable
    {
        private readonly string _message;
        private static ILog _log = LogManager.GetLogger("RunWatcher");
        private Stopwatch _watch;
        public RunWatcher(string message)
        {
            _message = message;
            _watch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _watch.Stop();
            _log.Debug("耗时: " + Convert.ToInt32(_watch.ElapsedMilliseconds) + "\r\n" + _message);
        }
    }
}