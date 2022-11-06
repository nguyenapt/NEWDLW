using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Events
{
    [ContentType(DisplayName = "Event page", GUID = "20AAA7FE-4997-4C13-83BD-A4764A48D9EB")]
    public class EventPage : GenericContainerPage
    {
        [CultureSpecific]
        [Display(Name = "Image (620 x 452)", Description = "Specify Image of the event(620 x 252  pixel)", GroupName = SharedTabs.Event, Order = 10)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [UIHint("UTC")]
        [Display(Name = "From", Description = "Specify the beginning time of the event", GroupName = SharedTabs.Event, Order = 15)]
        public virtual DateTime From { get; set; }

        [CultureSpecific]
        [UIHint("UTC")]
        [Display(Name = "To", Description = "Specify the end time of the event", GroupName = SharedTabs.Event, Order = 20)]
        public virtual DateTime To { get; set; }

        [CultureSpecific]
        [Display(Name = "Location", Description = "Specify location of the event", GroupName = SharedTabs.Event, Order = 25)]
        public virtual string Location { get; set; }

        [CultureSpecific]
        [Display(Name = "Event Link", Description = "Specify link to detailed event", GroupName = SharedTabs.Event, Order = 30)]
        public virtual Url EventLink { get; set; }

        [CultureSpecific]
        [Display(Name = "Event Link Text", Description = "Specify text for the link to detailed event", GroupName = SharedTabs.Event, Order = 35)]
        public virtual string LinkText { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            this.VisibleInMenu = false;
        }
    }
}