using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            var pos = 0;
            var maxPos = source.Count();
            while (pos < maxPos && source.Skip(pos).Any())
            {
                yield return source.Skip(pos).Take(chunksize);
                pos += chunksize;
            }
        }       
    }
}