using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.Downloads.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    public class DownloadsController : BlockController<DownloadsBlock>
    {
        private readonly IContentRepository _contentRepo;

        public DownloadsController(IContentRepository contentRepo)
        {
            _contentRepo = contentRepo;
        }

        public override ActionResult Index(DownloadsBlock currentBlock)
        {
            var viewModel = new DownloadsViewModel(currentBlock);
            var downloadItems = new List<DownloadFile>();
            if (!currentBlock.DownloadItems.IsNullOrEmpty())
            {
                foreach (var item in currentBlock.DownloadItems.FilteredItems)
                {
                    MediaData media;
                    if (!_contentRepo.TryGet(item.ContentLink, out media)) continue;
                    if (!(media is IContent)) continue;

                    downloadItems.Add(new DownloadFile(media.GetFileName(), media.GetFileExtension(), media.GetFileSize(), ((IContent)media).ContentLink));
                }
                viewModel.Items = downloadItems;
            }

            return PartialView("DownloadsBlock", viewModel);
        }
    }
}