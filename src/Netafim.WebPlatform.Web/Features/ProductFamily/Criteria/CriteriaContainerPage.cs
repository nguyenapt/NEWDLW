using EPiServer.Core;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    [ContentType(DisplayName = "Criteria Container page", GUID = "61ee4843-5971-4925-a7a5-799035da8c8e")]
    [AvailableContentTypes(Include = new[] { typeof(CriteriaPage) }, 
        Exclude = new[] { typeof(CriteriaContainerPage) })]
    public class CriteriaContainerPage : NoTemplatePageBase, ICanBeSearched
    {
        [Display(Order = 10, Name = "Display in family list header")]
        [CultureSpecific]
        public virtual bool DisplayInFamilyListHeader { get; set; }

        string ICanBeSearched.Title => Name;

        string ICanBeSearched.Summary => string.Empty;

        string ICanBeSearched.Keywords => string.Empty;

        ContentReference ICanBeSearched.Image => ContentReference.EmptyReference;
    }
}