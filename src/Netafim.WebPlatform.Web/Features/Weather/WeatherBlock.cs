using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Netafim.WebPlatform.Web.Core;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Core.Shell;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features._Shared.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.Weather
{
    [ContentType(DisplayName = "Weather block", GroupName = GroupNames.Overview, GUID = "442aae49-8074-4c49-b76b-e4dd8195a043")]
    public class WeatherBlock : BaseBlock, IComponent
    {
        public string ComponentName => this.GetComponentName();
        
        [Display( Name = "Show the floating weather", Order = 20, GroupName = SharedTabs.FloatingSettings)]
        public virtual bool DisplayFloating { get; set; }

        [Display(Name = "Padding to for the floating weather (Pixel)", Order = 30, GroupName = SharedTabs.FloatingSettings)]
        public virtual int PaddingTop { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            this.PaddingTop = 200;
        }
    }
}