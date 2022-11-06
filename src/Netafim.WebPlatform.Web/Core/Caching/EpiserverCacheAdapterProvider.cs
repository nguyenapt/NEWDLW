using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;

namespace Netafim.WebPlatform.Web.Core.Caching
{
    public class EpiserverCacheAdapterProvider : ICacheProvider
    {
        private readonly ISynchronizedObjectInstanceCache _objectInstanceCache;
        private readonly string[] _cacheDependencyKeys;

        public EpiserverCacheAdapterProvider(ISynchronizedObjectInstanceCache objectInstanceCache, IContentCacheKeyCreator cacheKeyCreator)
        {
            _cacheDependencyKeys = new[] { cacheKeyCreator.VersionKey };
            _objectInstanceCache = objectInstanceCache;
        }

        public T Get<T>(string key)
        {
            var value = _objectInstanceCache.Get(key);
            return value != null ? (T)value : default(T);
        }

        public bool Set<T>(string key, T value, double? time = null)
        {
            var cachePolicy = time.HasValue
                    ? new CacheEvictionPolicy(System.TimeSpan.FromSeconds(time.Value), CacheTimeoutType.Absolute)
                    : CacheEvictionPolicy.Empty;

            _objectInstanceCache.Insert(key, value, cachePolicy);
            return true;
        }

        public void Add(string key, object objectToCache, CacheOptions cacheOptions)
        {
            var evictionPolicy = new CacheEvictionPolicy(cacheOptions.CacheTime, cacheOptions.TimeoutType, _cacheDependencyKeys);
            _objectInstanceCache.Insert(key, objectToCache, evictionPolicy);
        }

        public void Remove(string key)
        {
            _objectInstanceCache.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            var tryGetValue = _objectInstanceCache.Get(key);
            if (tryGetValue == null)
            {
                value = null;
                return false;
            }

            value = tryGetValue;
            return true;
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            var tryGetValue = _objectInstanceCache.Get(key);
            if (tryGetValue == null || !(tryGetValue is T))
            {
                value = default(T);
                return false;
            }

            value = (T)tryGetValue;
            return true;
        }
    }
}
