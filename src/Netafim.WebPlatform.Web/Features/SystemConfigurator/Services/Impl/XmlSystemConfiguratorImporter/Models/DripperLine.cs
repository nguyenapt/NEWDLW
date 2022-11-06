namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl.XmlSystemConfiguratorImporter.Models
{
    public class DripperLine : Product
    {
        public decimal FlowRate { get; set; }

        public decimal EmiterSpacing { get; set; }

        public int NumberOfLaterals { get; set; }

        public decimal FlowVariation { get; set; }
    }
}