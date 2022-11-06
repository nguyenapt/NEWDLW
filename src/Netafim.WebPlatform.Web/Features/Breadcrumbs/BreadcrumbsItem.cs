using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Breadsrumbs
{
    public class BreadcrumbsItem
    {
        public BreadcrumbsItem(PageData page)
        {
            Page = page;
        }

        public PageData Page { get; set; }

    }
}