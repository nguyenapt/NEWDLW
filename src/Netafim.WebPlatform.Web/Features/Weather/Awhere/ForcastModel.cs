using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Weather.Awhere
{
    public class ForecastModel
    {
        [JsonProperty("forecasts")]
        public IEnumerable<Forecast> Forecasts { get; set; }
    }

    public class Forecast
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("forecast")]
        public IEnumerable<ForecastsData> Forecasts { get; set; }
    }

    public class ForecastsData
    {
    
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("conditionsCode")]
        public string ConditionsCode { get; set; }

        [JsonProperty("conditionsText")]
        public string ConditionsText { get; set; }

        [JsonProperty("temperatures")]
        public Temperatures Temperatures { get; set; }

        [JsonProperty("precipitation")]
        public Precipitation Precipitation { get; set; }

        [JsonProperty("sky")]
        public Sky Sky { get; set; }

        [JsonProperty("solar")]
        public Solar Solar { get; set; }

        [JsonProperty("relativeHumidity")]
        public Relativehumidity RelativeHumidity { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("dewPoint")]
        public Dewpoint DewPoint { get; set; }
    }
}