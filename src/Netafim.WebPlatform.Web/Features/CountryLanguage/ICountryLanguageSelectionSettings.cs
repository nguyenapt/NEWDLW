using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public interface ICountryLanguageSelectionSettings
    {
       IList<LanguageLinkItem> LocalSites { get; }

    }
}