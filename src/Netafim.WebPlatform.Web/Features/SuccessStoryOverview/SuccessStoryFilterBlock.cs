using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    [ContentType(DisplayName = "Success Story Overview Block", GUID = "2eae7c39-077e-4d3a-91d2-a8196cd9cb70", Description = "Success stories overview filter block")]
    public class SuccessStoryFilterBlock : ListingBaseBlock, IComponent
    {
        [Display(Order = 10)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [Display(Order = 20)]
        [CultureSpecific]
        public virtual int PageSize { get; set; }

        public string ComponentName => this.GetComponentName();
    }
}