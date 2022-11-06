using System.Globalization;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms.Search;
using Dlw.EpiBase.Content.Infrastructure.Mvc;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;

namespace Netafim.WebPlatform.Web.Features.Error
{
    public class ErrorController : Controller
    {
        private readonly IPageService _pageService;
        private readonly UrlResolver _urlResolver;
        private readonly ILanguageSegmentMatcher _languageSegmentMatcher;

        public ErrorController(IPageService pageService, UrlResolver urlResolver, ILanguageSegmentMatcher languageSegmentMatcher)
        {
            _pageService = pageService;
            _urlResolver = urlResolver;
            _languageSegmentMatcher = languageSegmentMatcher;
        }

        public ActionResult NotFound()
        {
            var pageReference = _pageService.GetPageReference<NotFoundPage>();

            string requestedLanguage;
            if (!ParseLanguageFromRequestingUrl(out requestedLanguage))
            {
                requestedLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }

            var url = _urlResolver.GetUrl(pageReference, requestedLanguage);

            if (string.IsNullOrWhiteSpace(url))
            {
                // fallback to default 404 (en)
                url = Url.Action("Default", "NotFound");
            }

            return new TransferResult(url);
        }

        private bool ParseLanguageFromRequestingUrl(out string parsedLanguage)
        {
            parsedLanguage = null;
            var segments = Request.RawUrl.Split('/');
            var languageSegment = segments.Length > 1 ? segments[1] : string.Empty;

            if (string.IsNullOrWhiteSpace(languageSegment))
            {
                return false;
            }

            return _languageSegmentMatcher.TryGetLanguageId(languageSegment, out parsedLanguage);
        }
    }
}