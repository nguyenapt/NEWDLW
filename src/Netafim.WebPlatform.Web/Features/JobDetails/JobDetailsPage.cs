using System;
using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features.JobFilter;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Editors;

namespace Netafim.WebPlatform.Web.Features.JobDetails
{
    [ContentType(DisplayName = "Job Details Page", GUID = "6c33a175-a4f4-4edb-a0b6-dd3a509f01f2", Description = "Component allows the user to view the details of a postion and apply for it", GroupName = GroupNames.JobInformation)]
    public class JobDetailsPage : GenericContainerPage
    {
        [Display(Description = "The text's displayed as the watermark of the page", Order = 5)]
        [CultureSpecific]
        public virtual string Watermark { get; set; }

        [Display(Name = "Turn on parallax watermark effect", Description = "The parallax effect is applied to the watermark as in the movie, defalut turn OFF effect", Order = 7)]
        [CultureSpecific]
        public virtual bool OnParallaxEffect { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [UIHint(UIHint.Textarea)]
        public override string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Country", GroupName = SystemTabNames.Content, Order = 20)]
        [SelectOne(SelectionFactoryType = typeof(CountrySelectionFactory))]
        public virtual string Country { get; set; }

        [CultureSpecific]
        [Display(Description = "Introduction", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual XhtmlString Introduction { get; set; }

        [CultureSpecific]
        [Display(Name = "Job application form", Description = "Generate job application form overlay", GroupName = SystemTabNames.Content, Order = 40)]
        [AllowedTypes(typeof(GeneralFormContainerBlock))]
        [ContentAreaMaxItems(1)]
        public virtual ContentArea JobApplicationForm { get; set; }

        [CultureSpecific]
        [Display(Description = "Location", GroupName = SystemTabNames.Content, Order = 50)]
        [AutoSuggestSelection(typeof(JobLocationSelectionQuery))]
        public virtual string Location { get; set; }

        [CultureSpecific]
        [Display(Description = "Department", GroupName = SystemTabNames.Content, Order = 60)]
        [AutoSuggestSelection(typeof(JobDepartmentSelectionQuery))]
        public virtual string Department { get; set; }

        [CultureSpecific]
        [UIHint("UTC")]
        [Display(Name = "Posting date", Description = "Posting date", GroupName = SystemTabNames.Content, Order = 70)]
        public virtual DateTime Postingdate { get; set; }

        [CultureSpecific]
        [UIHint("UTC")]
        [Display(Name = "End date", Description = "End date", GroupName = SystemTabNames.Content, Order = 80)]
        public virtual DateTime EndDate { get; set; }

        [CultureSpecific]
        [Display(Name = "Job schedule", Description = "Job schedule", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual string JobSchedule { get; set; }

        [CultureSpecific]
        [Display(Description = "Position", GroupName = SystemTabNames.Content, Order = 100)]
        [AutoSuggestSelection(typeof(JobPositionSelectionQuery))]
        public virtual string Position { get; set; }
    }
}