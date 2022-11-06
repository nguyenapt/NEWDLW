using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.Downloads.Models;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Downloads
{
    public class DownloadsContainerController : BlockController<DownloadsContainerBlock>
    {
        public override ActionResult Index(DownloadsContainerBlock currentBlock)
        {
            var viewModel = new DownloadsContainerViewModel(currentBlock);
           
            return PartialView(string.Format(Global.Constants.AbsoluteViewPathFormat, "Downloads", "DownloadsContainerBlock"), viewModel);
        }
    }
}