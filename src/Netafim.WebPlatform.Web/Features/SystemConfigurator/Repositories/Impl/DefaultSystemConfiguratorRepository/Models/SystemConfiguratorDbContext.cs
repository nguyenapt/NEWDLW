using System.Data.Entity;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models
{
    public class SystemConfiguratorDbContext : DbContext
    {
        public SystemConfiguratorDbContext() : base("name=EPiServerDB")
        { }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<CropEntity> Crops { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<ContentEntity> Contents { get; set; }

        public DbSet<ContactEntity> Contacts { get; set; }

        public DbSet<WaterSourceEntity> WaterSources { get; set; }

        public DbSet<FiltrationTypeEntity> FiltrationTypes { get; set; }

        public DbSet<DecisionTreeEntity> DecisionTrees { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("systemconfig");
        }
    }
}