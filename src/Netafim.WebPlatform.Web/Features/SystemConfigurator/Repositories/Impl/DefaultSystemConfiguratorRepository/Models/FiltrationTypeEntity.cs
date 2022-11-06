using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("FiltrationType")]
    public class FiltrationTypeEntity : SystemConfiguratorEntityBase
    {
        public string Name { get; set; }
    }
}