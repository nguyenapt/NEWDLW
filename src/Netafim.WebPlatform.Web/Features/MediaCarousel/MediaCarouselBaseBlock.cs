using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.MediaCarousel
{
    public abstract class MediaCarouselBaseBlock : BlockData, IMediaCarousel
    {
        [CultureSpecific]
        [Display(Description = "Carousel title", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Carousel description", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Text { get; set; }

        [CultureSpecific]
        [Display(Description = "This is the URL that the anchor will go to.", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual Url Link { get; set; }

        [CultureSpecific]
        [Display(Name = "Link text", GroupName = SystemTabNames.Content, Order = 35)]
        public virtual string LinkText { get; set; }

        [CultureSpecific]
        [Display(Description = "The headline display in pagination", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual XhtmlString Quote { get; set; }

        [UIHint(UIHint.Image)]
        [CultureSpecific]
        [Display(Name = "Image (1440x620)", Description = "Image display as image carousel and full width (1440x620)", GroupName = SystemTabNames.Content, Order = 50)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(1440, 620)]
        public virtual ContentReference Image { get; set; }

        [UIHint(UIHint.Video)]
        [CultureSpecific]
        [Display(Name = "Video(mp4, upto 100 Mb)", Description = "Video", GroupName = SystemTabNames.Content, Order = 60)]
        [AllowedTypes(typeof(VideoData))]
        public virtual ContentReference Video { get; set; }

        [CultureSpecific]
        [Display(Name = "Auto play & loop", Description = "Auto play and loop the video", GroupName = SystemTabNames.Content, Order = 70)]
        public virtual bool OnAutoPlay { get; set; }

        [CultureSpecific]
        [Display(Name = "Show content while playing", Description = "Show description content while playing the video", GroupName = SystemTabNames.Content, Order = 80)]
        public virtual bool ShowContentWhilePlaying { get; set; }
    }
}