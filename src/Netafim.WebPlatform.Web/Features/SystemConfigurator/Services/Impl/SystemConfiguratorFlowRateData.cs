using System.Globalization;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class SystemConfiguratorFlowRateData
    {
        public Region Region { get; set; }

        public Crop Crop { get; set; }

        public int PlotArea { get; set; }

        public int RowSpacing { get; set; }

        public int MaxAllowedIrrigationTimePerDay { get; set; }

        public int WeeklyIrrigationInterval { get; set; }

        public FiltrationType FiltrationType { get; set; }

        public WaterSource WaterSource { get; set; }

        public CultureInfo Culture { get; set; }
    }
}