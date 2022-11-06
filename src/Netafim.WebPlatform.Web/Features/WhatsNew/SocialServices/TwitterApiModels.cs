using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class TwitterTokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
    public class TwitterFeedItemModel
    {
        [JsonProperty("created_at")]
        public string CreatedDate { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("id_str")]
        public string Id { get; set; }

        public DateTime CreateDateTime
        {
            get
            {
                DateTime dt;
                if (!string.IsNullOrEmpty(CreatedDate)
                   && DateTime.TryParseExact(CreatedDate, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    return dt;
                }
                return DateTime.MinValue;
            }
        }
    }
}