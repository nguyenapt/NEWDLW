using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("DecisionTree")]
    public sealed class DecisionTreeEntity : SystemConfiguratorEntityBase
    {
        public CropEntity Crop { get; set; }

        public RegionEntity Region { get; set; }

        public SystemConfiguratorEntityBase Entity { get; set; }
    }
}