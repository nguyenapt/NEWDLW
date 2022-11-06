using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{

    [ContentType(DisplayName = "System Configurator Example Preview Block", GUID = "43590e58-871e-4774-ae39-a27c85c2b04d", GroupName = GroupNames.Configurator)]
    public class SystemConfiguratorExamplePreviewBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Name = "Title", Order = 30)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Sub Title", Order = 40)]
        public virtual string SubTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Description", Order = 50)]
        [TinyMceSettings(typeof(TinyMceBaseConfiguration))]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Thumbnail (300x340)", Order = 60)]
        [AllowedTypes(typeof(ImageFile))]
        [ImageMetadata(300,340)]
        public virtual ContentReference Thumbnail { get; set; }

        [CultureSpecific]
        [Display(Name = "Image example (800x600)", Order = 60)]
        [AllowedTypes(typeof(ImageFile))]
        [ImageMetadata(800, 600)]
        public virtual ContentReference Image { get; set; }

        public string ComponentName => this.GetComponentName(this.Title);
    }
}