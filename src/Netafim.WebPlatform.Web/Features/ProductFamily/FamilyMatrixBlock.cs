using EPiServer.DataAnnotations;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    [ContentType(DisplayName = "Family matrix block", GroupName = GroupNames.Products)]
    public class FamilyMatrixBlock : ListingBaseBlock, IComponent 
    {
        [Display(Order = 10)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Order = 20)]
        [UIHint(UIHint.LongString)]
        public virtual string SubTitle { get; set; }
        
        [CultureSpecific]
        [Display(Order = 30, Name = "Search Result Description")]
        [UIHint(UIHint.LongString)]
        public virtual string SearchResultDescription { get; set; }

        public string ComponentName => this.GetComponentName();
    }
}