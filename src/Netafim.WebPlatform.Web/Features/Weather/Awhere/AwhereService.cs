using DbLocalizationProvider;
using EPiServer.Logging;
using Netafim.WebPlatform.Web.Core.Caching;
using Netafim.WebPlatform.Web.Core.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Netafim.WebPlatform.Web.Features.Weather.Awhere
{
    public class AwhereService : RestServiceBase<AwhereApiOptions>, IWeatherService
    {
        protected readonly ICacheProvider CacheProvider;
        protected readonly IWeatherSettings WeatherSettings;
        private static ILogger _logger = LogManager.GetLogger(typeof(AwhereService));

        public AwhereService(AwhereClient client,
            AwhereApiOptions restApiOptions,
            IEnumerable<IHttpClientInterceptor> restInterceptors,
            ICacheProvider cacheProvider,
            IWeatherSettings weatherSettings)
            : base(client, restApiOptions, restInterceptors)
        {
            this.CacheProvider = cacheProvider;
            this.WeatherSettings = weatherSettings;
        }

        public async Task<IEnumerable<WeatherInformation>> ForcastAsync(double lat, double lng, DateTime from, DateTime to, int blockSize = 24)
        {
            var cacheKey = $"WeatherForecast_{lat}_{lng}_{from.Date.Ticks}_to_{to.Date.Ticks}";

            var cachedWeather = this.CacheProvider.Get<IEnumerable<WeatherInformation>>(cacheKey);

            if(cachedWeather == null)
            {
                var field = await this.CreateFieldAsync(lat, lng);

                if (field != null && field.Links != null && field.Links.Forcast != null)
                {
                    await this.Intercept();

                    var forecastUri = CreateFieldUrl(field, from, to, blockSize);

                    var forecasts = await this.GetResultAsync<ForecastModel>(forecastUri);

                    cachedWeather = Map(forecasts);

                    this.CacheProvider.Set(cacheKey, cachedWeather, this.WeatherSettings.CachedTime);
                }
            }

            return cachedWeather ?? Enumerable.Empty<WeatherInformation>();
        }

        public virtual async Task<TodayWeatherInformation> ForcastTodayAsync(double lat, double lng, DateTime today)
        {
            var cacheKey = $"WeatherToday_{lat}_{lng}_{today.Date.Ticks}";

            var cachedWeather = this.CacheProvider.Get<TodayWeatherInformation>(cacheKey);

            if(cachedWeather == null)
            {
                var field = await this.CreateFieldAsync(lat, lng);

                if (field != null && field.Links != null && field.Links.Forcast != null)
                {
                    await this.Intercept();

                    var forecastUri = CreateFieldUrl(field, today, null, 1);

                    var todayForecast = await this.GetResultAsync<Forecast>(forecastUri);
                                        
                    if (todayForecast == null)
                    {
                        _logger.Error($"Can not get the weather forecast of day {today.ToLongTimeString()}");
                        throw new Exception($"Can not get the weather forecast of day {today.ToLongTimeString()}");
                    }

                    var currentForecast = todayForecast.Forecasts.FirstOrDefault(t => t.StartTime < today && t.EndTime > today);

                    cachedWeather = new TodayWeatherInformation()
                    {
                        AverageTemperature = (int)(Math.Ceiling((currentForecast.Temperatures.max + currentForecast.Temperatures.min) / 2)),
                        Date = today
                    };

                    this.CacheProvider.Set(cacheKey, cachedWeather, this.WeatherSettings.CachedTime);
                }
            }


            return cachedWeather;
        }
        

        protected virtual string CreateFieldUrl(AwhereFieldResponse createdField, DateTime from, DateTime? to, int blockSize = 24)
        {
            if (to.HasValue)
            {
                return $"{createdField.Links.Forcast.Href}/{from.ToString("yyyy-MM-dd")},{to.Value.ToString("yyyy-MM-dd")}?blockSize={blockSize}&useLocalTime=true";
            }

            return $"{createdField.Links.Forcast.Href}/{from.ToString("yyyy-MM-dd")}?blockSize={blockSize}&useLocalTime=true";
        }

        protected virtual IEnumerable<WeatherInformation> Map(ForecastModel forecast)
        {
            if (forecast != null)
            {
                foreach (var f in forecast.Forecasts)
                {
                    var firstForecast = f.Forecasts.FirstOrDefault();

                    if (firstForecast != null)
                    {
                        var variances = new List<WeatherVariance>
                        {
                            new WeatherVariance() { Value = (int)Math.Ceiling((decimal)firstForecast.RelativeHumidity.average) + " %", Description = LocalizationProvider.Current.GetString(() => Labels.Humidity) },
                            new WeatherVariance() { Value = (int)Math.Ceiling(firstForecast.DewPoint.amount) + " " + firstForecast.DewPoint.units, Description = LocalizationProvider.Current.GetString(() => Labels.Transpiration) },
                            new WeatherVariance() { Value = (int)Math.Ceiling(firstForecast.Wind.average) + " " + firstForecast.Wind.units, Description = LocalizationProvider.Current.GetString(() => Labels.Wind) },
                        };

                        yield return new WeatherInformation()
                        {
                            Date = DateTime.Parse(f.Date),
                            HighTemperature = (int)Math.Ceiling(firstForecast.Temperatures.max),
                            LowTemperature = (int)Math.Ceiling(firstForecast.Temperatures.min),
                            TemperatureDescription = firstForecast.ConditionsText,
                            Variances = variances
                        };
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        protected virtual async Task<AwhereFieldResponse> CreateFieldAsync(double lat, double lng)
        {
            await this.Intercept();

            var field = new AwhereFieldRequest()
            {
                Id = Guid.NewGuid().ToString(),
                Acres = 100,
                CenterPoint = new CenterPoint() { Latitude = lat, Longitude = lng },
                FarmId = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            return await this.PostAsync<AwhereFieldResponse>("/v2/fields", field);
        }
    }
}