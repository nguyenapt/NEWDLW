using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("Filtration")]
    public class FiltrationEntity : ProductEntity
    {
        public decimal FlowRate { get; set; }

        public string TypeOfFiltration { get; set; }
        
        public FiltrationTypeEntity FiltrationType { get; set; }
        
        public WaterSourceEntity WaterSource { get; set; }

        public string FamilyName { get; set; }
    }
}