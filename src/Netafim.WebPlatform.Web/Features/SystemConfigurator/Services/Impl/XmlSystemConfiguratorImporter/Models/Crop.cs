namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models
{
    public class Crop : IIdentifiable
    {
        public string Key { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public decimal CropFactor { get; set; }
    }
}