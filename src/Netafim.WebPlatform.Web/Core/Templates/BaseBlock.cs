using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;
using EPiServer.Shell.ObjectEditing;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public abstract class BaseBlock : BlockData
    {
        /// <summary>
        /// The text rendered in the left side of component.
        /// </summary>
        [Display(Name = "Floating navigation text", Description = "This tool enhances the claritiy of the user in which component or section on a page", Order = 8)]
        [CultureSpecific]
        public virtual string VerticalText { get; set; }

        /// <summary>
        /// Watermark label rendered in the background.
        /// </summary>
        [Display(Description = "The text's displayed as the watermark of the section block", Order = 10)]
        [CultureSpecific]
        public virtual string Watermark { get; set; }

        [Display(Name = "Turn on parallax watermark effect", Description = "The parallax effect is applied to the watermark as in the movie, defalut turn OFF effect", Order = 15)]
        [CultureSpecific]
        public virtual bool OnParallaxEffect { get; set; }

        [Display(Name = "Unique anchor id for this section", Order = 20, GroupName = SharedTabs.Reference)]
        [RegularExpression("^[a-zA-Z0-9-]*$")]
        [CultureSpecific]
        public virtual string AnchorId { get; set; }

        [Display(Name = "Background color of block", Order = 30, Description = "Config the background color of block")]
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(BackgroundColorFactory))]
        public virtual BackgroundColor BackgroundColor { get; set; }
    }
}