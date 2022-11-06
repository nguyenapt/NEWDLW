namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models
{
    public abstract class Product : Content, IIdentifiable
    {
        public string CatalogNumber { get; set; }

        public string Name { get; set; }
    }
}