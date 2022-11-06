using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.CropsOverview;

namespace Netafim.WebPlatform.Web.Features.GenericListing
{
    [ContentType(DisplayName = "Generic Listing block", GroupName = GroupNames.Listers, GUID = "eb827ee3-8078-4432-9d43-eceda564c689", Description = "The component allows webmaster can list all children under own page.")]
    public class GenericListingBlock : BaseBlock, IComponent
    {
        [Display(Name = "Search root", Description = "Config the search root that will be find all contents under that root, Default is current page", Order = 30, GroupName = SharedTabs.Search)]
        [CultureSpecific]
        [AllowedTypes(typeof(PageBase), RestrictedTypes = new System.Type[] { typeof(CropsPage)})]
        public virtual ContentReference SearchRoot { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public string ComponentName => this.GetComponentName();
    }
}