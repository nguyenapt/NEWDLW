using System.Web.Mvc;
using EPiServer;

namespace Netafim.WebPlatform.Web.Features.GenericCTA.Helpers
{
    public interface IUrlLinkFactory
    {
        string CreateLink(UrlHelper url, GenericCTABlock block);
        bool IsSatisfied(Url url);
    }
}