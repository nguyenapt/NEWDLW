using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.Search
{
    public class GenericSearchResult
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public ContentReference Image { get; set; }
    }
}