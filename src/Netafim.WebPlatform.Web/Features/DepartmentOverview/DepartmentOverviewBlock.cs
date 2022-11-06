using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.DepartmentOverview
{

    [ContentType(DisplayName = "Department overview block", GUID = "fd8aa9e4-a69d-412b-9eb3-555952ab5458", GroupName = GroupNames.Overview, Description = "Overview of a department")]
    public class DepartmentOverviewBlock : ItemBaseBlock
    {

        [CultureSpecific]
        [Display(Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Department", GroupName = SystemTabNames.Content, Order = 30)]
        [AutoSuggestSelection(typeof(JobDepartmentSelectionQuery))]
        public virtual string Department { get; set; }

        [CultureSpecific]
        [Display(Name = "Thumbnail image (620x452)", Order = 40)]
        [AllowedTypes(typeof(ImageFile))]
        [ImageMetadata(620, 452)]
        public virtual ContentReference Thumbnail { get; set; }

        [CultureSpecific]
        [Display(Name = "Thumbnail image (78x78)", Order = 50)]
        [AllowedTypes(typeof(ImageFile))]
        [ImageMetadata(78, 78)]
        public virtual ContentReference Icon { get; set; }

        [CultureSpecific]
        [Display(Order = 60)]
        [TinyMceSettings(typeof(BoldTinyMceConfiguration))]
        public virtual XhtmlString Description { get; set; }
    }
}