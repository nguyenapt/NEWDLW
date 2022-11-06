using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System.Globalization;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public class CountryLanguageController : Controller
    {
        private readonly CultureInfo _corporateCulture = new CultureInfo("en");
        private readonly ICountryLanguageSelectionRepo _countryLangRepo;
        public CountryLanguageController(ICountryLanguageSelectionRepo countryLangRepo)
        {
            _countryLangRepo = countryLangRepo;
        }

        public ActionResult Index()
        {
            var viewModel = new CountryLanguageSelectionsModel
            {
                CorporateSiteUrl = GetCorporateSiteUrl(),
                LocalSites = _countryLangRepo.GetLocalSites()
            };

            return PartialView("CountryLanguageSelections", viewModel);
        }

        private string GetCorporateSiteUrl()
        {
            var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            return urlResolver.GetUrl(ContentReference.StartPage, _corporateCulture.Name);
        }
    }
}