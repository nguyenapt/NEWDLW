using System.Collections.Generic;
using System.Web.Routing;
using EPiServer;
using EPiServer.Security;
using EPiServer.Shell.Navigation;

namespace Netafim.WebPlatform.Web.Features._Shared.Shell
{
    [MenuProvider]
    public class ToolsMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var mainToolsMenu = new SectionMenuItem("Tools", "/global/tools")
            {
                IsAvailable = ((RequestContext request) => PrincipalInfo.Current.HasPathAccess(UriSupport.Combine("/Tools", "")))
            };

            return new MenuItem[]
            {
                mainToolsMenu
            };
        }
    }
}