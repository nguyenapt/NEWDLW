using Dlw.EpiBase.Content.Infrastructure.Maintenance.Warmup;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Error
{
    [ContentType(DisplayName = "404 Error", Description = "This page will be shown when a user tries to visit an unknown page. This page contains a search bar and allows to configure additional components.", GUID = "ec9d4256-204a-4e27-8790-08ded33ea9e8", GroupName = GroupNames.Infrastructure)]
    public class NotFoundPage : PageBase, IPreload
    {
        [AllowedTypes(typeof(IComponent))]
        [CultureSpecific]
        public virtual ContentArea Content { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            VisibleInMenu = false;
        }
    }
}