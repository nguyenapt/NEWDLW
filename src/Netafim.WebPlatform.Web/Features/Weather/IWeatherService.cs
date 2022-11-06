using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netafim.WebPlatform.Web.Features.Weather
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherInformation>> ForcastAsync(double lat, double lng, DateTime from, DateTime to, int blockSize = 24);

        Task<TodayWeatherInformation> ForcastTodayAsync(double lat, double lng, DateTime today);
    }

    public interface IWeatherSettings
    {
        int CachedTime { get; }
    }
    
    public class WeatherVariance
    {
        public virtual string Value { get; set; }

        public virtual string Description { get; set; }
    }
    
    public class WeatherInformation
    {
        public DateTime Date { get; set; }

        public double LowTemperature { get; set; }

        public double HighTemperature { get; set; }

        public string TemperatureDescription { get; set; }

        public IEnumerable<WeatherVariance> Variances { get; set; }
    }

    public class TodayWeatherInformation
    {
        public DateTime Date { get; set; }

        public int AverageTemperature { get; set; }
    }
}
