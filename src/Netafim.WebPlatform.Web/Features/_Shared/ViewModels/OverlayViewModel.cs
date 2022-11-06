using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features._Shared.ViewModels
{
    public class OverlayViewModel
    {
        public OverlayViewModel(string popupId)
        {
            PopupId = popupId;
        }

        public string PopupId { get; }

        public ContentArea Forms { get; set; }
    }
}