using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.MediaCarousel;
using Netafim.WebPlatform.Web.Features.SuccessStoryOverview;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.SuccessStory
{
    [ContentType(DisplayName = "Success Story Page", GUID = "514f7c61-3c67-4785-9415-98330c8f0138", Description = "")]
    public class SuccessStoryPage : GenericContainerPage, IMediaCarousel
    {
        [CultureSpecific]
        [Display(Description = "Carousel title", GroupName = SystemTabNames.Content, Order = 10)]
        public override string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Link text", GroupName = SystemTabNames.Content, Order = 15)]
        public virtual string LinkText { get; set; }

        [CultureSpecific]
        [Display(Description = "Carousel description", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Text { get; set; }
        
        [CultureSpecific]
        [Display(Description = "The headline display in pagination", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual XhtmlString Quote { get; set; }

        [UIHint(UIHint.Image)]
        [CultureSpecific]
        [Display(Name= "Image (1440 x 620)", Description = "Image display as image carousel and full width (1440 x 620)", GroupName = SystemTabNames.Content, Order = 60)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(1440, 620)]
        [Required]
        public virtual ContentReference Image { get; set; }

        [UIHint(UIHint.Video)]
        [CultureSpecific]
        [Display(Name = "Video(mp4, upto 100 Mb)", Description = "Video", GroupName = SystemTabNames.Content, Order = 70)]
        [AllowedTypes(typeof(VideoData))]
        public virtual ContentReference Video { get; set; }

        [CultureSpecific]
        [Display(Name = "Auto play & loop", Description = "Auto play and loop the video", GroupName = SystemTabNames.Content, Order = 80)]
        public virtual bool OnAutoPlay { get; set; }

        [CultureSpecific]
        [Display(Name = "Show content while playing", Description = "Show description content while playing the video", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual bool ShowContentWhilePlaying { get; set; }

        #region Content overview

        [CultureSpecific]
        [Display(Name="Project date", Description = "The success stories are ordered by 'project date' by default", GroupName = SharedTabs.SuccessStoryContentOverview, Order = 10)]
        public virtual DateTime ProjectDate { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(CropSelectionFactory))]
        [Display(Name="Crop", Description = "Crop ID", GroupName = SharedTabs.SuccessStoryContentOverview, Order = 20)]
        public virtual int CropId { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(CountrySelectionFactory))]
        [Display(Description = "Countries", GroupName = SharedTabs.SuccessStoryContentOverview, Order = 30)]
        public virtual string Country { get; set; }

        [CultureSpecific]
        [Display(Name="Boosted from", Description = "Boosted from", GroupName = SharedTabs.SuccessStoryContentOverview, Order = 40)]
        public virtual DateTime BoostedFrom { get; set; }

        [CultureSpecific]
        [Display(Name = "Boosted to", Description = "Boosted to", GroupName = SharedTabs.SuccessStoryContentOverview, Order = 50)]
        public virtual DateTime BoostedTo { get; set; }

        #endregion
    }
}