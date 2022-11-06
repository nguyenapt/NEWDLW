using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    public abstract class SystemConfiguratorEntityBase
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Country discriminator, required for all entity types.
        /// </summary>
        [Required]
        public string Culture { get; set; }

        public virtual string Key { get; set; }
    }
}