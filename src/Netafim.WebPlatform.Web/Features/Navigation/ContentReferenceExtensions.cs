using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.Navigation
{
    public static class ContentReferenceExtensions
    {
        private static readonly IContentRepository _contentRepo = ServiceLocator.Current.GetInstance<IContentRepository>();

        public static bool IsActiveNavigationNode(this ContentReference navItemLink, ContentReference currentPageLink)
        {
            if (navItemLink == null) return false;
            if (navItemLink != null && navItemLink.Equals(currentPageLink)) return true;

            return _contentRepo.GetAncestors(currentPageLink).Any(x => x.ContentLink.Equals(currentPageLink));
        }
    }
}