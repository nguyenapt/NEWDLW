using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;

namespace Netafim.WebPlatform.Web.Features.Search
{
    public class SearchViewModel : IPageViewModel<SearchPage>
    {
        public string SearchText { get; set; }

        public int PageNumber { get; set; }

        public int? Category { get; set; }

        public int NumberOfSearchResultsToShowOnOnePage { get; set; }

        public int MaxNumberOfPagesToShow { get; set; }

        public string Culture { get; set; }

        public int TotalMatching { get; set; }

        public int TotalMatchingAcrossAllCategories { get; set; }

        public SearchPage Current { get; }

        public bool ValidSearch { get; set; }

        public IEnumerable<GenericSearchResult> SearchResults { get; set; }

        public IEnumerable<FacetValueViewModel> CategoryFacetValues { get; set; }

        #region CalculatedFields

        public bool IsThisTheLastPage => PageNumber * NumberOfSearchResultsToShowOnOnePage >= TotalMatching;

        public int FirstShownResultNumber => (PageNumber - 1) * NumberOfSearchResultsToShowOnOnePage + 1;

        public int LastShownResultNumber => Math.Min(PageNumber * NumberOfSearchResultsToShowOnOnePage, TotalMatching);

        public int HighestPageNumber => PageNumber <= MaxNumberOfPagesToShow
            ? Math.Min(MaxNumberOfPagesToShow,
                Convert.ToInt32(Math.Ceiling(TotalMatching / (decimal) NumberOfSearchResultsToShowOnOnePage)))
            : PageNumber;

        public int LowestPageNumber => Math.Max(1, HighestPageNumber - MaxNumberOfPagesToShow + 1);

        #endregion

        public SearchViewModel(SearchPage currentPage)
        {
            Current = currentPage;
        }
    }
}