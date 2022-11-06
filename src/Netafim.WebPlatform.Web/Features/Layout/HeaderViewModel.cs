using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class HeaderViewModel
    {
        public ContentReference LogoLink { get; set; }

        public ContentReference HeroLogoLink { get; set; }

        public IEnumerable<LinkViewModel> OtherIndustries { get; set; }

        public ContentReference CarreerLink { get; set; }

        public ISocialMediaSettings Socials { get; set; }

        public ContentReference ContactUsLink { get; set; }
    }
}