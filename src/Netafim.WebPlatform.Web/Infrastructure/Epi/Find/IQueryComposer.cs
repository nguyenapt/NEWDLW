using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Find
{
    public interface IQueryComposer
    {
        bool IsSatisfied(QueryViewModel query);

        FilterExpression<ICanBeSearched> Compose(QueryViewModel query);

        Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder> GetSortings(QueryViewModel query);
    }

    public abstract class QueryComposerBase<TListingBlock, TQuery> : IQueryComposer
        where TListingBlock : ListingBaseBlock
        where TQuery : QueryViewModel
    {
        protected readonly IContentLoader ContentLoader;

        protected QueryComposerBase(IContentLoader contentLoader)
        {
            this.ContentLoader = contentLoader;
        }

        public abstract FilterExpression<ICanBeSearched> Compose(QueryViewModel query);

        public virtual Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder> GetSortings(QueryViewModel query)
        {
            var sorts = new Dictionary<Expression<Func<ICanBeSearched, IComparable>>, SortOrder>
            {
                { m => m.Title, SortOrder.Ascending }
            };

            return sorts;
        }

        public virtual bool IsSatisfied(QueryViewModel query)
        {
            if (query.BlockId <= 0)
                throw new ArgumentException($"The BlockId of query can not be null", nameof(query));

            var currentBlock = this.ContentLoader.Get<IContent>(new ContentReference(query.BlockId));

            if (currentBlock == null)
                throw new Exception($"Can not load the content with Id {query.BlockId}");

            return currentBlock.GetOriginalType() == typeof(TListingBlock);
        }

        protected TQuery GetQueryModel(QueryViewModel query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryModel = query as TQuery;

            if (queryModel == null)
                throw new ArgumentException($"The expected type of query parameter is {nameof(TQuery)}");

            return queryModel;
        }
    }
}
