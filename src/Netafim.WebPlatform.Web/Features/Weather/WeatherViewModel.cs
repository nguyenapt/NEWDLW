using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Weather
{

    public class WeatherViewModel : IBlockViewModel<WeatherBlock>
    {
        public WeatherBlock CurrentBlock { get; }

        public IEnumerable<WeatherInformation> Weathers { get; }  
        
        public string Location { get; set; }

        public WeatherViewModel(WeatherBlock block, IEnumerable<WeatherInformation> weathers)
        {
            this.CurrentBlock = block;
            this.Weathers = weathers;
        }
    }

    public class FloatingWeatherViewModel : IBlockViewModel<WeatherBlock>
    {
        public WeatherBlock CurrentBlock { get; }

        public int Temperature { get; set; }

        public FloatingWeatherViewModel(WeatherBlock block)
        {
            this.CurrentBlock = block;
        }
    }
}