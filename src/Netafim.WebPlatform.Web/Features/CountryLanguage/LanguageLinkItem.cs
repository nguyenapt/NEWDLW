using EPiServer.Shell.ObjectEditing;

namespace Netafim.WebPlatform.Web.Features.CountryLanguage
{
    public class LanguageLinkItem
    {
        [AutoSuggestSelection(typeof(LanguageLinkSelectionQuery))]        
        public string Language { get; set; }

        public string Link { get; set; }
    }
}