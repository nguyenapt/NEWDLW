using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    [ContentType(DisplayName = "Job filter block", GUID = "c353c7d3-8e10-40ae-b0c6-701c94699c78")]
    public class JobFilterBlock : ListingBaseBlock, IComponent
    {
        [Display(Order = 10)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 20)]
        [UIHint(UIHint.LongString)]
        public virtual string SubTitle { get; set; }

        public string ComponentName => this.GetComponentName();
    }
}