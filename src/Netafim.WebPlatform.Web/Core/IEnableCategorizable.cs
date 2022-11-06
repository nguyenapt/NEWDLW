using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Core
{
    /// <summary>
    /// Marker interface to enable the Category property for a specific template.
    /// </summary>
    public interface IEnableCategorizable : IContent
    {
        CategoryList Category { get; }
    }
}