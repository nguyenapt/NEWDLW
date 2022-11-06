using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms;
using Dlw.EpiBase.Content.Cms.Extensions;
using Dlw.EpiBase.Content.Cms.Search;
using Dlw.EpiBase.Content.Find;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;
using EPiServer.Web.Routing;
using Microsoft.Ajax.Utilities;
using Netafim.WebPlatform.Web.Core.Facet;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;

namespace Netafim.WebPlatform.Web.Features.Search
{
    public class SearchController : BasePageController<SearchPage>
    {
        private readonly IPageService _pageService;
        private readonly IClient _searchClient;
        private readonly CategoryRepository _categoryRepository;
        private readonly IUserContext _userContext;
        private readonly IExtendedUrlResolver _extendedUrlResolver;
        private readonly UrlResolver _urlResolver;

        private const int NumberOfSearchResultsToShowOnOnePage = 20;
        private const int MaxNumberOfPagesToShow = 4;
        private const int MinSearchQueryLength = 3;

        public SearchController(
            IPageService pageService,
            IClient searchClient,
            CategoryRepository categoryRepository,
            IUserContext userContext,
            IExtendedUrlResolver extendedUrlResolver, 
            UrlResolver urlResolver)
        {
            _pageService = pageService;
            _searchClient = searchClient;
            _categoryRepository = categoryRepository;
            _userContext = userContext;
            _extendedUrlResolver = extendedUrlResolver;
            _urlResolver = urlResolver;
        }

        public ActionResult Index(SearchPage currentPage)
        {
            return View(GetEmptySearchViewModel(currentPage, true));
        }

        [HttpGet]
        public ActionResult KeywordSearch()
        {
            var culture = _userContext.CurrentLanguage.Name;
            var searchPageReference = _pageService.GetPageReference<SearchPage>();

            if (searchPageReference == null) return new EmptyResult();

            var url = _urlResolver.GetUrl(searchPageReference, culture,
                new VirtualPathArguments
                {
                    Action = nameof(Query)
                });

            return PartialView("_keywordSearch", url);
        }

        [HttpGet]
        public ActionResult Query(string searchText, int? page, int? category)
        {
            var searchPage = _pageService.GetPage<SearchPage>();

            if (!IsQueryValid(searchText))
                return View("Index", GetEmptySearchViewModel(searchPage, false));

            var pageNumberZeroBased = page - 1 ?? 0;
            pageNumberZeroBased = Math.Max(pageNumberZeroBased, 0);

            var culture = _userContext.CurrentLanguage.Name;

            //TODO: review wildcard performance
            var query = _searchClient.Search<ICanBeSearched>()
                .For(searchText)
                .InField(x => x.Title)
                .InField(x => x.Summary)
                .InField(x => x.Keywords)
                .WildCardQuery($"*{searchText}*", x => x.Title)
                .WildCardQuery($"*{searchText}*", x => x.Summary)
                .WildCardQuery($"*{searchText}*", x => x.Keywords)
                .TermsFacetFor(x => x.CategoriesFacet())
                .PublishedInLanguage(culture)
                .ExcludeDeleted();

            var totalMatchingAcrossAllCategories = query.Count();

            if (category != null)
            {
                query = query.FilterHits(x => x.CategoriesFacet().Match(category.Value.ToString()));
            }

            var result = query
                .Skip(NumberOfSearchResultsToShowOnOnePage * pageNumberZeroBased)
                .Take(NumberOfSearchResultsToShowOnOnePage)
                .GetContentResult();

            //We wrap the results in GenericSearchResult objects because we don't want to get the pages.
            var searchResult = result.Select(x => MapItemToGenericSearchResult(x, culture)).ToList();

            var currentPage = pageNumberZeroBased + 1;

            var model = new SearchViewModel(searchPage)
            {
                SearchText = searchText,
                PageNumber = currentPage,
                Category = category,
                SearchResults = searchResult,
                TotalMatching = result.TotalMatching,
                TotalMatchingAcrossAllCategories = totalMatchingAcrossAllCategories,
                NumberOfSearchResultsToShowOnOnePage = NumberOfSearchResultsToShowOnOnePage,
                MaxNumberOfPagesToShow = MaxNumberOfPagesToShow,
                ValidSearch = true
            };

            model.CategoryFacetValues = GetCategoryFacetValues(result);

            return View("Index", model);
        }

        public ActionResult MetaTags()
        {
            return PartialView("_metaTags");
        }

        private GenericSearchResult MapItemToGenericSearchResult(ICanBeSearched x, string culture)
        {
            return new GenericSearchResult
            {
                Title = x.Title,
                Summary = x.Summary,
                Image = x.Image,
                Url = _extendedUrlResolver.GetExternalUrl(x.GetContentReference(), new CultureInfo(culture))
            };
        }

        private IEnumerable<FacetValueViewModel> GetCategoryFacetValues(IContentResult<ICanBeSearched> result)
        {
            var facet = (TermsFacet) result.Facets["CategoriesFacet"];

            return facet.Terms.Select(x => new FacetValueViewModel
            {
                Key = x.Term,
                Label = _categoryRepository.Get(Int32.Parse(x.Term)).Name,
                Count = x.Count
            });
        }

        private SearchViewModel GetEmptySearchViewModel(SearchPage currentPage, bool searchSucceeded)
        {
            return new SearchViewModel(currentPage)
            {
                PageNumber = 1,
                SearchResults = null,
                SearchText = string.Empty,
                TotalMatching = 0,
                Category = null,
                ValidSearch = searchSucceeded
            };
        }

        private bool IsQueryValid(string query)
        {
            if(query.IsNullOrWhiteSpace() || query.Length < MinSearchQueryLength)
            {
                return false;
            }
            return true;
        }
    }
}