namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class SystemConfiguratorData
    {
        public int RegionId { get; set; }

        public int CropId { get; set; }

        public int PlotArea { get; set; }

        public int RowSpacing { get; set; }
        
        public int MaxAllowedIrrigationTimePerDay { get; set; }

        public int WeeklyIrrigationInterval { get; set; }

        public int FiltrationTypeId { get; set; }

        public int WaterSourceId { get; set; }
    }
}