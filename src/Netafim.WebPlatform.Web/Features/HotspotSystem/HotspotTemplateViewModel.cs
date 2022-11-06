using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.HotspotSystem
{
    public class HotspotTemplateViewModel
    {
        public HotspotPopupPage Template { get; set; }

        public IEnumerable<HotspotPopupNodePage> Nodes { get; set; }
    }
}