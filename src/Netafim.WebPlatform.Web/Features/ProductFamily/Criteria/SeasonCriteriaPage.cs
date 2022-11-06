using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;

namespace Netafim.WebPlatform.Web.Features.ProductFamily.Criteria
{
    [ContentType(DisplayName = "Season Criteria page", GUID = "9ebc66ba-748e-4119-be5c-54de55c9d451")]
    public class SeasonCriteriaPage : CriteriaPage
    {
        [SelectOne(SelectionFactoryType = typeof(SeasonFactory))]
        public virtual int SeasonValue { get; set; }        
    }
}