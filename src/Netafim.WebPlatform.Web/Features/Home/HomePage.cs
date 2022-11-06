using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Error;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily.Criteria;
using Netafim.WebPlatform.Web.Features.Search;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Home
{
    [ContentType(DisplayName = "Homepage", GUID = "666839D9-B7D2-46E0-A4EA-19F98303629A")]
    [AvailableContentTypes(
        Include = new[]
        {
            typeof(GenericContainerPage),
            typeof(SearchPage),
            typeof(CriteriaContainerPage),
            typeof(NotFoundPage)
        },
        ExcludeOn = new[]
        {
            typeof(ProductCatalogPage)
        })]
    public class HomePage : GenericContainerPage
    {
        [Ignore]
        public override bool DisplayInSidebarNavigation { get; set; }
        [Ignore]
        public override bool DisplaySidebarNavigation { get; set; }
    }
}