using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.ProductFamily
{
    public class FamilyMatrixQueryViewModel : QueryViewModel
    {        
        public int[] Criteria { get; set; }
        public int ProductCategoryId { get; set; }
        public int[] CriteriaTypeIds { get; set; }

    }
}