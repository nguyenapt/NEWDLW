using Netafim.WebPlatform.Web.Features.RichText.Models;

namespace Netafim.WebPlatform.Web.Features.Downloads.Models
{
    public class DownloadsContainerViewModel : RichTextContainerViewModel
    {
        public DownloadsContainerViewModel(DownloadsContainerBlock block)
        {
            Block = block;
        }

        public string Title
        {
            get
            {
                var downloadsContainer = Block as DownloadsContainerBlock;
                return downloadsContainer != null ? downloadsContainer.Title : string.Empty;
            }
        }
    }
}