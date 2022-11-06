using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Logging;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public class FindBlockService : IBlockService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(FindPageService));

        private readonly IClient _searchClient;
        private readonly IUserContext _userContext;

        public FindBlockService(IClient searchClient, IUserContext userContext)
        {
            _searchClient = searchClient;
            _userContext = userContext;
        }

        public IEnumerable<T> GetBlocks<T>(int blocksToFetch, Expression<Func<T, Filter>> filter) where T : BlockData
        {
            Language currentLanguage = Language.English;
            var query = _searchClient.Search<T>(currentLanguage)
                .PublishedInLanguage(_userContext.CurrentLanguage.Name);

            if (filter != null)
            {
                query = query.Filter<T>(filter);
            }

            return query
                .ExcludeDeleted()
                .Take(blocksToFetch)
                .GetContentResult();
        }
    }
}