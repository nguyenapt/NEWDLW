using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Features.HotspotSystem;
using Netafim.WebPlatform.Web.Features.SuccessStory;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class TemplateCoordinator : IViewTemplateModelRegistrator
    {
        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            viewTemplateModelRegistrator.Add(typeof(HotspotPopupPage), new TemplateModel
            {
                Tags = new[] { Global.ContentAreaTags.HomepageTemplate },
                AvailableWithoutTag = true,
                Path = "~/Features/HotspotSystem/Views/_hotspotPopupPage.cshtml"
            });

            viewTemplateModelRegistrator.Add(typeof(HotspotLinkPage), new TemplateModel
            {
                Tags = new[] { Global.ContentAreaTags.HomepageTemplate },
                AvailableWithoutTag = true,
                Path = "~/Features/HotspotSystem/Views/_hotspotLinkPage.cshtml"
            });
        }
    }
}