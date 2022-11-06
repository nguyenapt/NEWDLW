using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell
{
    /// <summary>
    /// Base controller to handle the listing block request
    /// </summary>
    /// <typeparam name="TListingBlock"></typeparam>
    /// <typeparam name="TSearchableContent"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    public abstract class ListingBaseBlockController<TListingBlock, TQuery, TSearchableContent> : PartialContentController<TListingBlock>
        where TListingBlock : ListingBaseBlock
        where TSearchableContent : ICanBeSearched
        where TQuery : QueryViewModel
    {
        protected readonly IContentLoader ContentLoader;
        protected readonly IPageService PageService;
        protected readonly IFindSettings FindSettings;

        protected ListingBaseBlockController(IContentLoader contentLoader,
            IPageService pageService,
            IFindSettings findSettings)
        {
            this.ContentLoader = contentLoader;
            this.PageService = pageService;
            this.FindSettings = findSettings;
        }

        /// <summary>
        /// Action method for ajax calling when using filter or search
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Search(TQuery query)
        {
            return this.PopulateView(query);
        }

        protected virtual ActionResult PopulateView(TQuery query)
        {
            var composer = this.GetQueryComposer(query);

            if (composer == null)
                throw new Exception("Can not find any statisfied query composer.");

            var block = this.ContentLoader.Get<IContentData>(new ContentReference(query.BlockId)) as TListingBlock;
            if (block == null)
                throw new Exception($"Can not find the block with type {typeof(TListingBlock).Name} and Id = {query.BlockId}");

            var queryResult = this.PageService.GetContentsWithSorting(this.FindSettings.MaxItemsPerRequest, composer.Compose(query).Expression, composer.GetSortings(query));

            IPagedList<TSearchableContent> pagedList = new PagedList<TSearchableContent>(queryResult.Cast<TSearchableContent>(), queryResult.TotalMatching, queryResult.TotalMatching, 1);
            var viewModel = new PaginableBlockViewModel<TListingBlock, TSearchableContent>(block, pagedList);

            return PartialView(ResultViewPath(block), viewModel);
        }

        protected virtual IQueryComposer GetQueryComposer(TQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var composers = ServiceLocator.Current.GetAllInstances<IQueryComposer>();

            if (composers == null || !composers.Any())
                throw new Exception("No QueryCompose is registered.");

            return composers.FirstOrDefault(s => s.IsSatisfied(query));
        }

        protected virtual string ResultViewPath(TListingBlock model)
        {
            return model.GetDefaultViewName();
        }
        protected virtual string FullViewPath(TListingBlock model)
        {
            return model.GetDefaultFullViewName();
        }
    }
}