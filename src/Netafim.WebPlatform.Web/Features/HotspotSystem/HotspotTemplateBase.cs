using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public abstract class HotspotTemplateBase : NoTemplatePageBase, IComponent, IHotspotTemplate
    {
        [Display(Name = "Unique anchor id for this section", Order = 5, GroupName = SystemTabNames.Content)]
        [RegularExpression("^[a-zA-Z0-9-]*$")]
        [CultureSpecific]
        public virtual string AnchorId { get; set; }

        [CultureSpecific]
        [Display(Description = "Title of hotspot template", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Subtitle of of hotspot template", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string SubTitle { get; set; }

        [CultureSpecific]
        [Display(Description = "Image display as background image.", GroupName = SystemTabNames.Content, Order = 30)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(Name = "Image width", Description = "width of background image", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual int ImageWidth { get; set; }

        [CultureSpecific]
        [Display(Name = "Image height", Description = "height of background image", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual int ImageHeight { get; set; }

        [CultureSpecific]
        [Display(Name = "Hotspot icon (56x56)", Description = "hotspot icon", GroupName = SystemTabNames.Content, Order = 60)]
        [AllowedTypes(typeof (ImageData))]
        [UIHint(UIHint.Image)]
        [ImageMetadata(56, 56)]
        public virtual ContentReference HotspotIcon
        {
            get
            {
                var icon = this.GetPropertyValue(t => t.HotspotIcon);
                if (icon != null)
                {
                    return icon;
                }
                var settings = ServiceLocator.Current.GetInstance<IHotspotSystemSettings>();
                return settings.HotspotIconFallback ?? ContentReference.EmptyReference;
            }
            set
            {
                this.SetPropertyValue(t => t.HotspotIcon, value);
            }
        }

        public virtual bool HasImage => !ContentReference.IsNullOrEmpty(Image);
        
        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}