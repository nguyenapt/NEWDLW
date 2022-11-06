using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.CropsOverview
{
    [ContentType(DisplayName = "Crops overview container", GUID = "04b2812b-bcd6-43da-bf02-43ded1d59028", Description = "", GroupName = GroupNames.CropsOverview)]
    public class CropsContainerBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(Description = "Crops overview title", GroupName = SystemTabNames.Content, Order = 15)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(Description = "Crops overview Subtitle", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Subtitle { get; set; }

        [AllowedTypes(typeof(IPreviewable))]
        [CultureSpecific]
        [Display(Description = "Crops overview items", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentArea Items { get; set; }

        [CultureSpecific]
        [Display(Description = "Explore our expertise CTA", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual ContentReference Link { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}