using Newtonsoft.Json;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{

    public class DealerGroupViewModel
    {
        [JsonProperty("category_icon")]
        public string Logo { get; set; }

        [JsonProperty("category_color")]
        public string Color { get; set; }

        [JsonProperty("category_pin")]
        public string Pin { get; set; }

        [JsonProperty("category_name")]
        public string Name { get; set; }

        [JsonProperty("category_type")]
        public string Type { get; set; }

        [JsonProperty("dealers")]
        public List<DealerDetailViewModel> Dealers { get; set; }

        public int TotalDealers
        {
            get
            {
                return this.Dealers != null ? this.Dealers.Count : 0;
            }
        }
    }
}