using DbLocalizationProvider;

namespace Netafim.WebPlatform.Web.Features.Search
{
    [LocalizedResource]
    public class Labels
    {
        public static string SearchPlaceholder => "Enter keywords..";

        public static string SearchBtnText => "Search site";

        public static string ResultsFound => "Showing {firstShownResultNumber}-{lastShownResultNumber} of {totalNumberOfResults} results for";

        public static string NoResultsFound => "No results..";

        public static string Page => "Page";

        public static string AllSections => "All sections";

        public static string Filter => "Filter by section";

        public static string ValidationMessage => "Please enter your keyword (min. 3 characters)";

        public static string KeywordSearchTitle => "What are you looking for?";

        public static string KeywordSearchPlaceholder => "Search keyword";

        public static string KeywordSearchButton => "Search";
    }
}