using System.Linq;
using System.Web.Mvc;
using Dlw.EpiBase.Content.Cms;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Features.CountryLanguage;

namespace Netafim.WebPlatform.Web.Features.Seo
{
    public class SeoController : Controller
    {
        private readonly IExtendedUrlResolver _extendedUrlResolver;
        private IContentRouteHelper _contentRouteHelper;
        private readonly IUserContext _userContext;
        private readonly ICountryLanguageSelectionSettings _countryLanguageSelectionSettings;

        public SeoController(IExtendedUrlResolver extendedUrlResolver, IUserContext userContext, ICountryLanguageSelectionSettings countryLanguageSelectionSettings)
        {
            _extendedUrlResolver = extendedUrlResolver;
            _userContext = userContext;
            _countryLanguageSelectionSettings = countryLanguageSelectionSettings;
        }

        public ActionResult Index()
        {
            var currentLanguageIsReleased = !_countryLanguageSelectionSettings.LocalSites?.Any(x => x.Language == _userContext.CurrentLanguage.ToString());

            return PartialView("_seoInformation", new SeoModel()
            {
                CurrentLanguageIsReleased = currentLanguageIsReleased ?? true
            });
        }

        public ActionResult OpenGraphTags()
        {
            _contentRouteHelper = ServiceLocator.Current.GetInstance<IContentRouteHelper>();
            return PartialView("_openGraphTags", _extendedUrlResolver.GetExternalUrl(_contentRouteHelper.Content.ContentLink, _userContext.CurrentLanguage));
        }
    }
}