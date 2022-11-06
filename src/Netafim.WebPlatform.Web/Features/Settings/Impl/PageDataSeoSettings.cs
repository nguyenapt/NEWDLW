using Dlw.EpiBase.Content.Infrastructure.Seo;
using EPiServer;

namespace Netafim.WebPlatform.Web.Features.Settings.Impl
{
    public class PageDataSeoSettings : PageDataSettings, ISeoSettings
    {
        public string TitleSuffix => SettingsPage.TitleSuffix;

        public PageDataSeoSettings(IContentRepository contentRepository) : base(contentRepository)
        {
        }
    }
}