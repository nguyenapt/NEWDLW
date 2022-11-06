using EPiServer.SpecializedProperties;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public interface ILinkItemMapper
    {
        IEnumerable<LinkViewModel> GetLinkItems(LinkItemCollection linkCollection);
    }
}