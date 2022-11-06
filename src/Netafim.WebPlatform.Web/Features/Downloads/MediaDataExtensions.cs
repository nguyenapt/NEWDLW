using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates.Media;
using System.IO;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    public static class MediaDataExtensions
    {
        public static string GetFileSize(this MediaData media)
        {
            if (media == null) return string.Empty;
            using (var stream = media.BinaryData.OpenRead())
            {
                return (stream.Length / 1024) + " KB";            
            }
        }

        public static string GetFileExtension(this MediaData media)
        {
            if (media == null || string.IsNullOrWhiteSpace(media.Name)) return string.Empty;
            var fileInfo = new FileInfo(media.Name);
            var fileExtension= fileInfo.Extension.ToUpper();
            return fileExtension.StartsWith(".") ? fileExtension.Remove(0, 1) : fileExtension;
        }

        public static string GetFileName(this MediaData media)
        {
            if (media == null || string.IsNullOrWhiteSpace(media.Name)) return string.Empty;
            var fileInfo = new FileInfo(media.Name);
            return fileInfo.Name.Contains(".") ? fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".")) : fileInfo.Name;
        }
    }
}