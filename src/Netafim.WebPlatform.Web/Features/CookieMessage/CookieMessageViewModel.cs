using DbLocalizationProvider;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.CookieMessage
{
    public class CookieMessageViewModel : ICTAComponent
    {
        public ContentReference CookieLink { get; set; }

        [Ignore]
        public string LinkText => LocalizationProvider.Current.GetString(() => Labels.CookieLinkMessage);

        [Ignore]
        public ContentReference Link => this.CookieLink;
    }
}