using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EPiServer.Core;
using EPiServer.Find.Api.Querying;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public interface IBlockService
    {
        /// <summary>
        /// Returns blocks for specific BlockData.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="blocksToFetch"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<T> GetBlocks<T>(int blocksToFetch, Expression<Func<T, Filter>> filter = null) where T : BlockData;

    }
}