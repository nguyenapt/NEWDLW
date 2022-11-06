namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models
{
    public abstract class Content : IIdentifiable
    {
        public string Key { get; set; }

        public int ContentReference { get; set; }
    }
}