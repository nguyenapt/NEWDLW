using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Templates.Media
{
    [ContentType(DisplayName = "Image File", GUID = "30c9f74c-1fae-468e-b71a-2b56f89070cf", Description = "Regular image file")]
    [MediaDescriptor(ExtensionString = "gif,jpg,jpeg,png,svg")]
    public class ImageFile : ImageData
    { }
}