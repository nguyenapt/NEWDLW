using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.RichText
{
    public class RichTextWhiteBoxController : BlockController<RichTextWhiteBoxBlock>
    {
        public override ActionResult Index(RichTextWhiteBoxBlock currentContent)
        {
            return PartialView(string.Format(Global.Constants.AbsoluteViewPathFormat, "Richtext", "_richTextWhiteBox"), currentContent);
        }
    }
}