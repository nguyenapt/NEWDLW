using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{

    [ContentType(DisplayName = "Sustainable item block", GUID = "616436a8-ba1e-4ffe-8c9a-9254da7fb3bf", GroupName = GroupNames.Overview)]
    public class SustainableItemBlock : GenericOverviewItemBlock
    {
        [CultureSpecific]
        [Display(Name = "Image (620x620)", GroupName = SystemTabNames.Content, Order = 20)]
        [ImageMetadata(620, 620)]
        public virtual ContentReference Image { get; set; }
        
        [Ignore]
        public override ContentReference Link { get; set; }

        [Ignore]
        public override string Title { get; set; }
    }
}