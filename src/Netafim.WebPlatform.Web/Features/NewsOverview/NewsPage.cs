using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    [ContentType(DisplayName = "News page", GUID = "20a926d9-7d41-4ff8-a9b4-c9ce9a6f5995")]
    public class NewsPage : GenericContainerPage
    {
        [CultureSpecific]
        [Display(Name = "Image (620 x 452)", Description = "Specify Image of the news(620 x 452  pixel)", Order = 10)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(Name = "News Date", Description = "Specify the date when the news is available",  Order = 15)]
        public virtual DateTime NewsDate { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            NewsDate = Created;
        }
    }
}