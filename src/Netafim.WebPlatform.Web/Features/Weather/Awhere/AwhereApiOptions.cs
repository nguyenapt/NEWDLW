using Dlw.EpiBase.Content.Infrastructure;
using Netafim.WebPlatform.Web.Core.Rest;
using Newtonsoft.Json;
using System;

namespace Netafim.WebPlatform.Web.Features.Weather.Awhere
{

    public class AwhereAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiredIn { get; set; }

        public DateTime Expired { get; set; }
    }

    public class AwhereApiOptions : BaseConfigurationManagerSettings, ISecuredApiOptions, IWeatherSettings
    {
        public string ClientId => GetAppSetting<string>("Awhere.ClientId");

        public string ClientSecret => GetAppSetting<string>("Awhere.ClientSecret");

        public string BaseAddress => GetAppSetting<string>("Awhere.ApiAddress");

        public int CachedTime => GetAppSetting<int>("Awhere.CachedTime");
    }

}