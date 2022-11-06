using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class FacebookTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
    public class FacebookFeed
    {
        public List<FacebookPostItem> Data;

        public object Paging;
    }

    public class FacebookPostItem
    {
        public string Id { get; set; }

        public string Message { get; set; }

        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }        
    }
}