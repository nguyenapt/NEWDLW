using Netafim.WebPlatform.Web.Core;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Shell;

namespace Netafim.WebPlatform.Web.Features.GenericOverview
{
    [ContentType(DisplayName = "Generic overview component", GroupName = GroupNames.Overview, Description = "Component that allows the webmaster to create an overview of services, challenges, Using an image and link to go to the detail page.")]
    public class GenericOverviewBlock : BaseBlock, IComponent, ICTAComponent
    {
        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(GroupName = SystemTabNames.Content, Order = 15)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.LongString)]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Description { get; set; }

        [AllowedTypes(typeof(GenericOverviewItemBlock))]
        [CultureSpecific]
        [Display(Description = "Overview items", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentArea Items { get; set; }

        [CultureSpecific]
        [Display(Description = "Link to internal content", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual ContentReference Link { get; set; }

        [CultureSpecific]
        [Display(Name = "Link Text", Description = "Specify Text for the link", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual string LinkText { get; set; }  
        
        [CultureSpecific]
        [Display(Name = "Mark the link is blue", Description = "Ticked is marking the link is blue style", GroupName = SystemTabNames.Content, Order = 65)]
        public virtual bool BlueLink { get; set; }

        [Display(Name = "Display mode", Order = 70, GroupName = SharedTabs.LayoutSettings)]
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(GenericOverviewDisplayingModeFactory))]
        public virtual GenericOverviewDisplayingMode DisplayingMode { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            DisplayingMode = GenericOverviewDisplayingMode.Carousel;
        }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName(Title);
    }
}