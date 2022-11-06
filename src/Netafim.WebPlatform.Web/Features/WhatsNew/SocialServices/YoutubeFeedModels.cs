using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class YoutubeFeedModels
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("items")]
        public IEnumerable<YoutubeFeedItem> Items { get; set; }

        public class YoutubeFeedItem
        {
            [JsonProperty("kind")]
            public string Kind { get; set; }

            [JsonProperty("etag")]
            public string Etag { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("snippet")]
            public ItemSnippet Snippet { get; set; }

            [JsonProperty("contentDetails")]
            public ItemContentDetails ContentDetails { get; set; }
        }

        public class ItemSnippet
        {
            [JsonProperty("publishedAt")]
            public DateTime PublishedAt { get; set; }

            [JsonProperty("channelId")]
            public string ChannelId { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("thumbnails")]
            public SnippetThumbnailMedium ThumbnailMedium { get; set; }

            [JsonProperty("channelTitle")]
            public string ChannelTitle { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            public class SnippetThumbnailMedium
            {
                [JsonProperty("medium")]
                public Thumbnai Medium { get; set; }
            }
        }

        public class Thumbnai
        {
            [JsonProperty("url")]
            public string Url { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
        }

        public class ItemContentDetails
        {
            [JsonProperty("upload")]
            public ContentUpload Upload { get; set; }

            public class ContentUpload
            {
                [JsonProperty("videoId")]
                public string VideoId { get; set; }
            }
        }
    }
}