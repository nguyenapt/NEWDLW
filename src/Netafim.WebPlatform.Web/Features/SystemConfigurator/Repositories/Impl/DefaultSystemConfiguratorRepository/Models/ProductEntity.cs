using System.ComponentModel.DataAnnotations.Schema;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    [Table("Product")]
    public abstract class ProductEntity : SystemConfiguratorEntityBase
    {
        public override string Key
        {
            get => CatalogNumber;
            set => CatalogNumber = value;
        }

        public string Name { get; set; }

        public string CatalogNumber { get; set; }

        public int ContentReference { get; set; }
    }
}