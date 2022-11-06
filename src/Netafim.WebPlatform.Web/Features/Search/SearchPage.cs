using System.ComponentModel.DataAnnotations;
using Dlw.EpiBase.Content.Infrastructure.Maintenance.Warmup;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Search
{
    [ContentType(DisplayName = "Search page", GUID = "2a5f7b4e-9aa6-4632-b7f0-0390d8dfe966", Description = "Page that allows you to search through the site's content.")]
    public class SearchPage : PageBase, IPreload
    {
        [Display(Description = "The text's displayed as the watermark of the page", Order = 10)]
        [CultureSpecific]
        public virtual string Watermark { get; set; }
    }
}