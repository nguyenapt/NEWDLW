using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public interface ICountryLanguageSelectionRepo
    {
        IList<LocalSiteItemViewModel> GetLocalSites();
    }
}