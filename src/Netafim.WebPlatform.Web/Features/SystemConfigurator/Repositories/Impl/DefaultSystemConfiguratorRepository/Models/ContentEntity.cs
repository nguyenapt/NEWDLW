using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("Content")]
    public class ContentEntity : SystemConfiguratorEntityBase
    {
        public int ContentReference { get; set; }
    }
}