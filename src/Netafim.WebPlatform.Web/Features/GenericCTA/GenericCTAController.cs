using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.GenericCTA
{
    public class GenericCTAController : BlockController<GenericCTABlock>
    {
        public override ActionResult Index(GenericCTABlock currentContent)
        {
            var view = currentContent.IsInlineMode ? "_inlineCTA" : "_genericCTA";
            return PartialView(view, currentContent);
        }
    }
}