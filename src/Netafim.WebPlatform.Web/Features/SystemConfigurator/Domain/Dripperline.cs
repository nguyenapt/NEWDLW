namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain
{
    public class Dripperline : Product
    {
        public int Id { get; set; }

        public decimal FlowRate { get; set; }

        public decimal EmiterSpacing { get; set; }

        public int NumberOfLaterals { get; set; }

        public decimal FlowVariation { get; set; }
    }
}