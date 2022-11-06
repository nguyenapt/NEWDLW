namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain
{
    public class Filtration : Product
    {
        public int Id { get; set; }

        public decimal FlowRate { get; set; }

        public FiltrationType FiltrationType { get; set; }

        public WaterSource WaterSource { get; set; }
    }
}