using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("Crop")]
    public class CropEntity : SystemConfiguratorEntityBase
    {
        public string Group { get; set; }

        public string Name { get; set; }

        public decimal CropFactor { get; set; }
    }
}