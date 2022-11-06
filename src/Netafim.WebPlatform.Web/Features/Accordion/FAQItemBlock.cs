using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Accordion
{
    [ContentType(DisplayName = "FAQs Item", GUID = "7093f37e-2bbe-465c-9db0-ddd6b3ac8444", Description = "Component that allows the webmaster to create an overview of FAQs", GroupName = GroupNames.Accordion)]
    public class FAQItemBlock : AccordionItemBase
    {
    }
}