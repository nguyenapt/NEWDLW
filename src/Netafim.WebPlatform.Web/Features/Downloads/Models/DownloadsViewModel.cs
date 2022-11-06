using Castle.Core.Internal;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.Downloads.Models
{
    public class DownloadsViewModel
    {
        public DownloadsBlock Block { get; set; }

        public IEnumerable<DownloadFile> Items { get; set; }

        public DownloadsViewModel(DownloadsBlock block)
        {
            Block = block;            
        }
    }

}