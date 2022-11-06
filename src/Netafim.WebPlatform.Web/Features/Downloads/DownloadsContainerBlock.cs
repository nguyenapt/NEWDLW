using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    [ContentType(DisplayName = "Downloads Container Block", GUID = "500DEA85-060E-4C04-9FC8-79800DE05A83", Description = "")]
    public class DownloadsContainerBlock : RichTextContainerBlock
    {
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "Specify title for the Download container.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Title { get; set; }
    }
}