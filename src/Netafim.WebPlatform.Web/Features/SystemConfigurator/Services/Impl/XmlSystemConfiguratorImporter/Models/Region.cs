namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models
{
    public class Region : IIdentifiable
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public int Eto { get; set; }

        public string Dealer { get; set; }

        public string DealerPhone { get; set; }

        public string NetafimSales { get; set; }

        public string NetafimPhone { get; set; }
    }
}