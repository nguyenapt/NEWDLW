using Castle.Core.Internal;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> AddIfNotExist<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict == null)
            {
                dict = new Dictionary<TKey, TValue>();
            }
            if (dict.ContainsKey(key)) return dict;
            dict.Add(key, value);
            return dict;
        }
    }
}