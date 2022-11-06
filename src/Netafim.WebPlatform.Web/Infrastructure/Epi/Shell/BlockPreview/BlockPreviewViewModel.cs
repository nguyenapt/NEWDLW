using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.BlockPreview
{
    public class BlockPreviewViewModel
    {
        public IContent Content { get; private set; }

        public BlockPreviewViewModel(IContent content)
        {
            this.Content = content;
        }
    }
}