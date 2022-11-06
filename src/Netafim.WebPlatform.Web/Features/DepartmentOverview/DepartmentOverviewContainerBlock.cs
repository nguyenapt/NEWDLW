using Dlw.EpiBase.Content.Infrastructure.TinyMce;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Infrastructure.Settings.TinyMce;
using System;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.DepartmentOverview
{
    [ContentType(DisplayName = "Department Overview Container block", GUID = "d2d8deab-b9da-461e-8e8b-b9405da63eff", GroupName = GroupNames.Overview, Description = "Component displays a visual overview of a number of departments to convince people to apply for a job")]
    public class DepartmentOverviewContainerBlock : BaseBlock, IComponent
    {
        [CultureSpecific]
        [Display(Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 30, Name = "Departments")]
        [AllowedTypes(typeof(DepartmentOverviewBlock))]
        public virtual ContentArea Departments { get; set; }

        [CultureSpecific]
        [Display(Name = "Application form", Order = 40)]
        [AllowedTypes(typeof(GeneralFormContainerBlock))]
        [ContentAreaMaxItems(1)]
        public virtual ContentArea OverlayContent { get; set; }

        [CultureSpecific]
        [Display(Name = "The anchor id of department filter block", Order = 50)]
        public virtual string FilterComponentAnchorId { get; set; }

        public string ComponentName => this.GetComponentName(this.Title);
    }
}