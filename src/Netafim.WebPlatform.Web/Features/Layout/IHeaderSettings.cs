using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public interface IHeaderSettings
    {
        ContentReference LogoLink { get; }

        ContentReference HeroLogolink { get; }

        ContentReference CarreerLink { get; }

        LinkItemCollection OtherIndustries { get; }

        ContentReference ContactUsLink { get; }
    }
}