using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell;
using System.Web.Mvc;
using EPiServer.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.GenericContainer
{
    [ContentOutputCache]
    public class GenericContainerController : BasePageController<GenericContainerPage>
    {
        public ActionResult Index(GenericContainerPage currentPage)
        {
            return View(currentPage);
        }
    }
}