using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Logging;
using EPiServer.Find.Api;

namespace Dlw.EpiBase.Content.Cms.Search
{
    public class FindPageService : IPageService
    {
        protected readonly ILogger _logger = LogManager.GetLogger(typeof(FindPageService));

        protected readonly IClient SearchClient;
        protected readonly IUserContext UserContext;
        protected readonly IContentRepository ContentRepository;
        protected readonly IFindSettings FindSettings;

        public FindPageService(IClient searchClient,
            IUserContext userContext,
            IContentRepository contentRepository,
            IFindSettings findSettings
            )
        {
            SearchClient = searchClient;
            UserContext = userContext;
            ContentRepository = contentRepository;
            FindSettings = findSettings;
        }

        public ContentReference GetPageReference<T>() where T : PageData
        {
            var result = SearchClient.Search<T>()
                .ExcludeDeleted()
                .PublishedInCurrentLanguage() // NEXT return unpublished items in edit mode
                .Take(1)
                .Select(x => new PageReference(x.PageLink.ID, x.PageLink.ProviderName))
                .StaticallyCacheFor(FindSettings.CacheDuration)
                .GetResult();

            if (result.TotalMatching == 0) return null;

            if (result.Hits.Count() > 1) throw new Exception($"More than 1 page found of type '{nameof(T)}'.");

            return result.Hits.First().Document;
        }

        public T GetPage<T>() where T : PageData
        {
            var pageReference = this.GetPageReference<T>();

            return ContentRepository.Get<T>(pageReference, new LoaderOptions() { LanguageLoaderOption.FallbackWithMaster(UserContext.CurrentLanguage) });
        }

        public IEnumerable<T> GetPages<T>(int pagesToFetch, Expression<Func<T, Filter>> filter) where T : PageData
        {
            return GetPagesInternal(pagesToFetch, filter);
        }

        public IEnumerable<T> GetPagesWithSorting<T, TProperty>(int pagesToFetch, Expression<Func<T, Filter>> filter,
            Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings) where T : PageData where TProperty : IComparable
        {
            return GetPagesInternalWithSorting(pagesToFetch, filter, sortings);
        }

        public IEnumerable<T> GetSiblingPages<T>(int pagesToFetch, PageReference currentPageReference) where T : PageData
        {
            var currentPage = ContentRepository.Get<T>(currentPageReference);
            var currentParentPage = ContentRepository.Get<PageData>(currentPage.ParentLink);

            var resultPages = GetPagesInternal<T>(
                FindSettings.MaxItemsPerRequest,
                x => x.ParentLink.Match(currentPage.ParentLink) & !x.ContentLink.Match(currentPageReference)
            );

            var filter = (FilterSortOrder)currentParentPage.Property["PageChildOrderRule"].Value;

            var sortedResultPages = SortPagesResult(filter, resultPages);

            return sortedResultPages.OfType<T>().Take(pagesToFetch);
        }

        public IContentResult<T> GetContents<T>(int pagesToFetch, Expression<Func<T, Filter>> filter = null, int? skip = null) where T : IContentData
        {
            return GetQuery<T>(pagesToFetch, filter, skip)
                .GetContentResult();
        }

        protected ITypeSearch<T> GetQuery<T>(int pagesToFetch, Expression<Func<T, Filter>> filter = null, int? skip = null) where T : IContentData
        {
            skip = skip ?? 0;
            return this.CreateQuery<T>(filter)
                .Skip(skip.Value)
                .Take(pagesToFetch)
                .StaticallyCacheFor(FindSettings.CacheDuration);
        }

        public IContentResult<T> GetContentsWithSorting<T, TProperty>(int pagesToFetch, Expression<Func<T, Filter>> filter = null,
            Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings = null, int? skip = null)
            where T : IContentData
            where TProperty : IComparable
        {
            skip = skip ?? 0;
            var result = this.CreateQuery<T>(filter)
                .Skip(skip.Value)
                .Take(pagesToFetch)
                .BuildSorting(sortings)
                .StaticallyCacheFor(FindSettings.CacheDuration)
                .GetContentResult();
            return result;
        }

        private IEnumerable<T> GetPagesInternalWithSorting<T, TProperty>(int pagesToFetch, Expression<Func<T, Filter>> filter, Dictionary<Expression<Func<T, TProperty>>, SortOrder> sortings)
            where T : PageData
            where TProperty : IComparable
        {
            return this.CreateQuery<T>(filter)
                .Take(pagesToFetch)
                .BuildSorting(sortings)
                .StaticallyCacheFor(FindSettings.CacheDuration)
                .GetPagesResult();
        }

        private PagesResult<T> GetPagesInternal<T>(int pagesToFetch, Expression<Func<T, Filter>> filter) where T : PageData
        {
            return this.CreateQuery<T>(filter)
                .OrderBy(x => x.StartPublish)
                .Take(pagesToFetch)
                .StaticallyCacheFor(FindSettings.CacheDuration)
                .GetPagesResult();
        }

        private ITypeSearch<T> CreateQuery<T>(Expression<Func<T, Filter>> filter) where T : IContentData
        {
            Language currentLanguage = Language.English; // NEXT use index in correct language, if available
            var query = SearchClient.Search<T>(currentLanguage)
                .CurrentlyPublished()
                .PublishedInCurrentLanguage();

            if (filter != null)
            {
                query = query.Filter<T>(filter);
            }
            return query
                .ExcludeDeleted();
        }

        private PageDataCollection SortPagesResult<T>(FilterSortOrder sortOrder, PagesResult<T> resultPages) where T : PageData
        {
            FilterPropertySort sorter;
            var sortedResultPages = new FilterEventArgs(resultPages.PageDataCollection);
            switch (sortOrder)
            {
                case FilterSortOrder.Alphabetical:
                    sorter = new FilterPropertySort("PageName", FilterSortDirection.Ascending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.CreatedDescending:
                    sorter = new FilterPropertySort("PageCreated", FilterSortDirection.Descending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.CreatedAscending:
                    sorter = new FilterPropertySort("PageCreated", FilterSortDirection.Ascending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.ChangedDescending:
                    sorter = new FilterPropertySort("PageChanged", FilterSortDirection.Descending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.Rank:
                    sorter = new FilterPropertySort("PageCreated", FilterSortDirection.Ascending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.PublishedAscending:
                    sorter = new FilterPropertySort("PageStartPublish", FilterSortDirection.Ascending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.PublishedDescending:
                    sorter = new FilterPropertySort("PageStartPublish", FilterSortDirection.Descending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.Index:
                    sorter = new FilterPropertySort("PagePeerOrder", FilterSortDirection.Ascending);
                    sorter.Filter(this, sortedResultPages);
                    break;
                case FilterSortOrder.None:
                    break;
            }

            return sortedResultPages.Pages;
        }
    }
}