using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("DripperLine")]
    public class DripperLineEntity : ProductEntity
    {
        public decimal FlowRate { get; set; }

        public decimal EmiterSpacing { get; set; }

        public int NumberOfLaterals { get; set; }

        public decimal FlowVariation { get; set; }
    }
}