using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("WaterSource")]
    public class WaterSourceEntity : SystemConfiguratorEntityBase
    {
        public string Name { get; set; }
    }
}