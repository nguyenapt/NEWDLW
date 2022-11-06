using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public class FooterViewModel
    {
        public ContentReference Logo { get; set; }
        public ContentReference Carreer { get; set; }
        public ContentReference ContactUs { get; set; }
        public IEnumerable<LinkViewModel> InternalLeadings { get; set; }
        public ISocialMediaSettings Socials { get; set; }
        public IEnumerable<LinkViewModel> SubFooter { get; set; }
    }
}