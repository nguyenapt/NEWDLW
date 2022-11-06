using Dlw.EpiBase.Content.Cms;
using EPiServer;
using EPiServer.Core;
using EPiServer.Personalization;
using EPiServer.Personalization.Providers.MaxMind;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Weather.Awhere;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Weather
{
    public class WeatherController : PartialContentController<WeatherBlock>
    {
        protected readonly IWeatherService WeatherService;
        protected readonly GeolocationProviderBase GeolocationProvider;
        protected readonly IContentLoader ContentLoader;
        private readonly IUserContext _userContext;

        public WeatherController(IWeatherService weatherService,
            GeolocationProviderBase geolocationProvider,
            IContentLoader contentLoader,
            IUserContext userContext)
        {
            this.WeatherService = weatherService;
            this.GeolocationProvider = geolocationProvider;
            this.ContentLoader = contentLoader;
            this._userContext = userContext;
        }

        public override ActionResult Index(WeatherBlock currentContent)
        {
            double lat, lng = 0;

            var location = this.GetLocation(out lat, out lng);
            
            var weathers = this.WeatherService.ForcastAsync(lat, lng, DateTime.Now, DateTime.Now.AddDays(3)).Result;

            var viewModel = new WeatherViewModel(currentContent, weathers)
            {
                Location = location?.ToUpper(this._userContext.CurrentLanguage)
            };

            return PartialView("_weather", viewModel);
        }

        public async Task<ActionResult> FloatingWeather(DateTime currentTime, int blockId)
        {
            if(blockId > 0)
            {
                var block = this.ContentLoader.Get<WeatherBlock>(new ContentReference(blockId));
                if(block != null && block.DisplayFloating)
                {
                    double lat, lng = 0;

                    this.GetLocation(out lat, out lng);

                    var foreCast = await this.WeatherService.ForcastTodayAsync(lat, lng, currentTime);

                    var viewModel = new FloatingWeatherViewModel(block)
                    {
                        Temperature = foreCast.AverageTemperature,
                    };

                    return PartialView("_floatingWeather", viewModel);
                }
            }

            return new EmptyResult();
        }

        private string GetLocation(out double lat, out double lng)
        {
            var latQuery = Request.QueryString["lat"];
            var lngQuery = Request.QueryString["lng"];

            if(!string.IsNullOrEmpty(latQuery) && !string.IsNullOrEmpty(lngQuery))
            {
                lat = double.Parse(latQuery);
                lng = double.Parse(lngQuery);

                return string.Empty;
            }
            else
            {
                var ipAddress = this.Request.GetClientFullIpAddress();

                var latlng = GeolocationProvider.Lookup(ipAddress);

                lat = latlng.Location.Latitude;
                lng = latlng.Location.Longitude;

                return latlng.Region;
            }
        }        
    }
}