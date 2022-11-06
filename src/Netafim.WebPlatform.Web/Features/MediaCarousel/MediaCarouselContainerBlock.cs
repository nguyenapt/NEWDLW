using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.SuccessStory;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    [ContentType(DisplayName = "Media Carousel Container Block", GUID = "b0080b5a-bf4d-4643-9770-54674036d2e5", Description = "Carousel functionality for Image, Video or Related success story", GroupName = GroupNames.MediaCarousel)]
    public class MediaCarouselContainerBlock : BaseBlock, IComponent, ICarouselMode
    {
        [Ignore]
        public override string Watermark { get; set; }

        [Ignore]
        public override BackgroundColor BackgroundColor { get; set; }

        [Ignore]
        public override bool OnParallaxEffect { get; set; }

        [CultureSpecific]
        [Display(Name = "Display as carousel boxed ", Description = "Default full width mode", GroupName = SystemTabNames.Content, Order = 5)]
        public virtual bool IsBoxMode { get; set; }

        [CultureSpecific]
        [Display(Name = "Carousel items", Description = "Carousel items", GroupName = SystemTabNames.Content, Order = 10)]
        [AllowedTypes(new [] {typeof(VideoCarouselBlock), typeof(ImageCarouselBlock), typeof(SuccessStoryPage) })]
        [ContentAreaMaxItems(3)]
        public virtual ContentArea Items { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}