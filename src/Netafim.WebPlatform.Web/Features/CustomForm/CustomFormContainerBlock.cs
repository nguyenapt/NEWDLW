using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.FormContainerBlock;
using Netafim.WebPlatform.Web.Features.RichText.Models;

namespace Netafim.WebPlatform.Web.Features.CustomForm
{
    [ContentType(DisplayName = "Custom Form Container Block", GUID = "2b6f4510-f71e-4b99-8223-63c4f772c4d9", Description = "The webmaster must be able to build a custom form depending on his needs", GroupName = GroupNames.Form)]
    public class CustomFormContainerBlock : GeneralFormContainerBlock, IRichTextColumnComponent
    {
        [Display(Name = "Icon (50x70)", Description = "The icon of custom form (50x70)", GroupName = SystemTabNames.Content)]
        [AllowedTypes(typeof(ImageData))]
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        [ImageMetadata(50, 70)]
        public virtual ContentReference FormIcon { get; set; }

        [Ignore]
        public override string MandatoryInformation { get; set; }

        [Ignore]
        public override string PrivacyStatement { get; set; }
    }
}