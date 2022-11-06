using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("Region")]
    public class RegionEntity : SystemConfiguratorEntityBase
    {
        public string Name { get; set; }

        public int Eto { get; set; }

        public string Dealer { get; set; }

        public string DealerPhone { get; set; }

        public string NetafimSales { get; set; }

        public string NetafimPhone { get; set; }
    }
}