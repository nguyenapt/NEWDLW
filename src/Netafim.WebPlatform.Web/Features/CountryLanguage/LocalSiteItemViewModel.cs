namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public class LocalSiteItemViewModel
    {
        public string Url { get; set; }
        public string Country { get; set; }

        public string Language { get; set; }

        public LocalSiteItemViewModel() { }
        public LocalSiteItemViewModel(string country, string lang, string url)
        {
            Country = country;
            Language = lang;
            Url = url;
        }
    }
}