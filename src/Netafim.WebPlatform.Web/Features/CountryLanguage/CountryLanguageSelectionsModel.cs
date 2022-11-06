using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public class CountryLanguageSelectionsModel
    {     
        public string CorporateSiteUrl { get; set; } 
        public IList<LocalSiteItemViewModel> LocalSites { get; set; }
    }  
}