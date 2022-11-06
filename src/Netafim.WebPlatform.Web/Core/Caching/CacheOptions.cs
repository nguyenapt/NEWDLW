using EPiServer.Framework.Cache;
using System;

namespace Netafim.WebPlatform.Web.Core.Caching
{
    public class CacheOptions
    {
        public CacheOptions(CacheTimeoutType timeoutType, TimeSpan cacheTime)
        {
            TimeoutType = timeoutType;
            this.CacheTime = cacheTime;
        }
        public CacheTimeoutType TimeoutType { get; set; }

        public TimeSpan CacheTime { get; set; }
    }
}
