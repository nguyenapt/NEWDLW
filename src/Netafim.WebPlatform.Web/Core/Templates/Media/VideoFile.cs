using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Templates.Media
{
    [ContentType(DisplayName = "Video File", GUID = "32e3e80d-ede3-4745-ba18-bdfe60338b14", Description = "Regular video file")]
    [MediaDescriptor(ExtensionString = "mp4")]
    public class VideoFile : VideoData
    {
    }
}