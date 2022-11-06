using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Services;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.FormContainerBlock
{
    [ContentType(DisplayName = "General Form Container Block", GUID = "264abf75-63f2-4dac-a106-f3dc02e67bc8", Description = "The block can be embedded in an overlay or in a page, using for Contact form, Job application form", GroupName = GroupNames.Form)]
    public class GeneralFormContainerBlock : BaseBlock, IComponent, IEmailTemplateSettings
    {
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Description = "Title of contact form", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Description = "Description of contact form", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string Description { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Mandatory Info", Description = "All fields marked with * are mandatory", GroupName = SystemTabNames.Content, Order = 35)]
        public virtual string MandatoryInformation { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Privacy statement", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string PrivacyStatement { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(EPiServer.Forms.Implementation.Elements.FormContainerBlock))]
        [ContentAreaMaxItems(1)]
        [Display(Name = "Form", Description = "Form container block", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual ContentArea FormContainer { get; set; }

        /// <summary>
        /// Get component name on page, if Title is null or empty return name of content
        /// </summary>
        public  string ComponentName => this.GetComponentName(Title);

        #region Email template settings

        [CultureSpecific]
        [Display(Description = "This is a subject on mail", GroupName = SharedTabs.EmailTemplate, Order = 20)]
        public virtual string Subject { get; set; }

        [CultureSpecific]
        [Display(Description = "Title of email", GroupName = SharedTabs.EmailTemplate, Order = 30)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(Name = "Send to Cc addresses (separate by ; semicolon mark)", Description = "List of CC addresses, separate by semicolon(;) mark", GroupName = SharedTabs.EmailTemplate, Order = 40)]
        public virtual string CcAddresses { get; set; }

        [CultureSpecific]
        [Display(Name = "Send to Bcc addresses (separate by ; mark)", Description = "List of CC addresses, separate by semicolon(;) mark", GroupName = SharedTabs.EmailTemplate, Order = 50)]
        public virtual string BccAddresses { get; set; }

        #endregion
    }
}