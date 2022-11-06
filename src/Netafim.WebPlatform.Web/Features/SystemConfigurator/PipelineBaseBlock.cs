using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator
{
    public abstract class PipelineBaseBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Name = "Title", Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Name = "Description", Order = 30)]
        [TinyMceSettings(typeof(BoldTinyMceConfiguration))]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(Name = "Next step page", Order = 40, Description = "Link to next step when summit button is triggered.")]
        public virtual ContentReference Next { get; set; }

        public string ComponentName => this.GetComponentName(this.Title);
    }
}