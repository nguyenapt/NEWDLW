using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.Weather.Awhere
{
    public class AwhereFieldRequest
    {
        #region Properties

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("acres")]
        public double? Acres { get; set; }

        [JsonProperty("centerPoint")]
        public CenterPoint CenterPoint { get; set; }

        [JsonProperty("farmId")]
        public string FarmId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        #endregion Properties
    }

    public class AwhereFieldResponse : AwhereFieldRequest
    {
        [JsonProperty("_links")]
        public AwhereFieldLinks Links { get; set; }
    }

    public class AwhereFieldLinks
    {
        [JsonProperty("awhere:forecasts")]
        public Link Forcast { get; set; }
    }

    public class Link
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class CenterPoint
    {

        #region Constructors

        public CenterPoint()
        {
        }

        public CenterPoint(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        #endregion Constructors

        #region Properties

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        #endregion Properties

        #region Overrides
        public override string ToString()
        {

            return String.Format("Lat: {0} - Long: {1}", Latitude, Longitude);
        }
        #endregion Overrides
    }

    public class AwhereClient : HttpClient
    {
        public AwhereClient(AwhereApiOptions apiOptions)
        {
            this.BaseAddress = new Uri(apiOptions.BaseAddress);
        }
    }

    public class Location
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string fieldId { get; set; }
    }

    public class Temperatures
    {
        public float max { get; set; }
        public float min { get; set; }
        public string units { get; set; }
    }

    public class Precipitation
    {
        public float amount { get; set; }
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Solar
    {
        public float amount { get; set; }
        public string units { get; set; }
    }

    public class Relativehumidity
    {
        public float? max { get; set; }
        public float? min { get; set; }
        public float? average { get; set; }
    }

    public class Wind
    {
        public float morningMax { get; set; }
        public float dayMax { get; set; }
        public float average { get; set; }
        public string units { get; set; }
    }

    public class Dewpoint
    {
        public float amount { get; set; }
        public string units { get; set; }
    }

    public class Sky
    {
        public double cloudCover { get; set; }
        public double sunshine { get; set; }
    }

    public class Meantemp
    {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Maxtemp
    {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Mintemp
    {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Minhumidity
    {
        public float average { get; set; }
        public float stdDev { get; set; }
    }

    public class Maxhumidity
    {
        public float average { get; set; }
        public float stdDev { get; set; }
    }

    public class Dailymaxwind
    {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }

    public class Averagewind
    {
        public float average { get; set; }
        public float stdDev { get; set; }
        public string units { get; set; }
    }
}