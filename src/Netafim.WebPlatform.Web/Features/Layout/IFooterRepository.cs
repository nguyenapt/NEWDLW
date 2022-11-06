using Netafim.WebPlatform.Web.Features.Navigation;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Layout
{
    public interface IFooterRepository
    {
        IEnumerable<NavigationPage> GetInternalLeadings();
    }
}
