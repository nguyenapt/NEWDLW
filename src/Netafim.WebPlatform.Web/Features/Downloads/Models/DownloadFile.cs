using EPiServer.Core;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Downloads.Models
{

    public class DownloadFile
    {
        public DownloadFile()
        {
        }

        public DownloadFile(string name, string type, string size, ContentReference fileContentRef)
        {
            Name = name;
            Type = type;
            Size = size;
            ContentLink = fileContentRef;
        }   
        public string Name { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public ContentReference ContentLink { get; set; }
    }

}