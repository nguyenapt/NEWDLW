using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using EPiServer.Web.Mvc;

namespace Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.BlockPreview
{
    [TemplateDescriptor(Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcController,
        Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
        AvailableWithoutTag = false)]
    public class BlokkPreviewController : Controller, IRenderTemplate<BlockData>
    {
        public ActionResult Index(IContent currentContent)
        {
            var model = new BlockPreviewViewModel(currentContent);

            return View("_blockPreview", model);
        }
    }
}