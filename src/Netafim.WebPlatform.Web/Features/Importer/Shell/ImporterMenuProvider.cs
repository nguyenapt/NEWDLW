using System.Collections.Generic;
using System.Web.Routing;
using EPiServer.Shell.Navigation;

namespace Netafim.WebPlatform.Web.Features.Importer.Shell
{
    [MenuProvider]
    public class ImporterMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var importerMenuItem = new UrlMenuItem("Import", "/global/tools/importer", "/Importer/Index")
            {
                IsAvailable = ((RequestContext request) => true),
                SortIndex = 100
            };

            return new MenuItem[]
            {
                importerMenuItem
            };
        }
    }
}