using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features._Shared.ViewModels
{
    public class DisabledComponentViewModel
    {
        public IContent Content { get; set; }

        public string Reason { get; set; }
    }
}