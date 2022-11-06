using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public class DealerDetailViewModel
    {
        [JsonProperty("logo")]
        public string Logo { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }
        
        public string Email { get; set; }

        public string Website { get; set; }

        public string Direction { get; set; }
        
        [JsonProperty("lng")]
        public double Longtitude { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }
    }
}