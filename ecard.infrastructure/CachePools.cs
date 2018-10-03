using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Ecard.Services
{
    public class CachePools
    {
        static readonly ICacheManager CacheManager = CacheFactory.GetCacheManager();
        public static object GetData(string key)
        {
            return CacheManager.GetData(key);
        }
        public static void AddCache(string key, object item)
        {
            CacheManager.Add(key, item);
        }

        public static void Remove(string key)
        {
            CacheManager.Remove(key);
        }
    }
}