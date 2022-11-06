using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    public interface ICanBeSearched : IContentData
    {
        string Title { get; }

        string Summary { get; }

        string Keywords { get; }

        ContentReference Image { get; }
    }
}