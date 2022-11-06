using System.Globalization;
using EPiServer.Core;

namespace Dlw.EpiBase.Content.Cms
{
    public interface IExtendedUrlResolver
    {
        string GetExternalUrl(ContentReference contentReference, CultureInfo contentLanguage, string action = null, object routeData = null);
    }
}