using System;
using System.Net;
using System.Threading;
using Microsoft.Practices.Unity;
using log4net;

namespace Ecard.Services
{
    public interface ICacheService
    {
        void Refresh(string key);
    }

    public class LocalCacheService : ICacheService
    {
        public void Refresh(string key)
        {
            CachePools.Remove(key);
        }
    }
    public class RemoteCacheService : ICacheService
    {
        private readonly string _url;
        private static ILog _logger = log4net.LogManager.GetLogger(typeof(RemoteCacheService));
        public RemoteCacheService(string url)
        {
            _url = url;
        }

        public void Refresh(string key)
        {
            ThreadPool.QueueUserWorkItem((o) =>
                                             {
                                                 try
                                                 {
                                                     using (WebClient client = new WebClient())
                                                     {
                                                         client.DownloadString(string.Format(_url, key));
                                                     }
                                                 }
                                                 catch (Exception ex)
                                                 {
                                                     _logger.Error("清理远程缓存", ex);
                                                 }
                                             });
        }
    }
    public class CompositeCacheService : ICacheService
    {
        private readonly IUnityContainer _unityContainer;

        public CompositeCacheService(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public void Refresh(string key)
        {
            var cacheServices = _unityContainer.ResolveAll<ICacheService>();
            foreach (var cacheService in cacheServices)
            {
                try
                {
                    cacheService.Refresh(key);
                }
                catch
                {
                }
            }
        }
    }
}
