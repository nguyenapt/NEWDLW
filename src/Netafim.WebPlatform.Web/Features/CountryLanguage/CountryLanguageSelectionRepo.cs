using System;
using Castle.Core.Internal;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public class CountryLanguageSelectionRepo : ICountryLanguageSelectionRepo
    {
        private readonly IContentRepository _contentRepo;
        private readonly ICountryLanguageSelectionSettings _countryLangSettings;
        private readonly ILanguageBranchRepository _langBranchRepo;
        private readonly UrlResolver _urlResolver;
        private readonly string _defaultLanguage = "en";

        public CountryLanguageSelectionRepo(IContentRepository contentRepo, ICountryLanguageSelectionSettings countryLangSettings,
            ILanguageBranchRepository langBranchRepos, UrlResolver urlResolver)
        {
            _contentRepo = contentRepo;
            _countryLangSettings = countryLangSettings;
            _langBranchRepo = langBranchRepos;
            _urlResolver = urlResolver;
        }

        public IList<LocalSiteItemViewModel> GetLocalSites()
        {
            var result = new List<LocalSiteItemViewModel>();

            var enabledLanguagesForLocalSites = _langBranchRepo.ListEnabled().Where(x => x.IsLocalSite() && !x.Culture.IsNeutralCulture);
            if (enabledLanguagesForLocalSites.IsNullOrEmpty())
            {
                return result;
            }

            var externalLocalSites = _countryLangSettings.LocalSites;
            var fallbackHomePage = _urlResolver.GetUrl(ContentReference.StartPage, _defaultLanguage);

            foreach (var language in enabledLanguagesForLocalSites)
            {
                var localSiteHomePage = GetLocalSiteHomePage(language);
                var externalLocalSite = GetExternalLocalSite(language, externalLocalSites);
                var pageUrlWithCulture = externalLocalSite ?? _urlResolver.GetUrl(localSiteHomePage, language.LanguageID);

                string countryName, languageName;
                ExtractCountryAndLang(language, out countryName, out languageName);

                result.Add(new LocalSiteItemViewModel(countryName, languageName, pageUrlWithCulture));
            }
            return result;
        }

        private void ExtractCountryAndLang(LanguageBranch lang, out string countryName, out string languageName)
        {
            if (!lang.IsLocalSite())
            {
                countryName = languageName = string.Empty;
                return;
            }
            var regionInfo = new RegionInfo(lang.Culture.LCID);
            countryName = regionInfo.EnglishName;
            languageName = lang.Culture.Parent != CultureInfo.InvariantCulture ? lang.Culture.Parent.EnglishName : lang.Culture.EnglishName;
        }

        private string GetExternalLocalSite(LanguageBranch lang, IList<LanguageLinkItem> externalLocalSites)
        {
            var localSiteSetting = externalLocalSites?.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Language) && lang.LanguageID.Equals(x.Language));
            return localSiteSetting?.Link;
        }

        private ContentReference GetLocalSiteHomePage(LanguageBranch lang)
        {
            //TODO: update later when making decission about multiply sites solution.
            return ContentReference.StartPage;
        }
    }
}