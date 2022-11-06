using EPiServer;

namespace Netafim.WebPlatform.Web.Core.Caching
{
    public interface ICacheProvider
    {
        T Get<T>(string key);

        bool Set<T>(string key, T value, double? time = null);

        bool TryGetValue(string key, out object value);
        void Add(string key, object value, CacheOptions cacheOptions);
        bool TryGetValue<T>(string key, out T value);
        void Remove(string key);
    }
}
