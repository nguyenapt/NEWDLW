using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Extensions;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public class HotspotSystemController : ContentController<IHotspotTemplate>
    {
        public ActionResult Index(IHotspotTemplate currentPage)
        {
            return View(currentPage.GetDefaultViewName(), currentPage);
        }
    }
}