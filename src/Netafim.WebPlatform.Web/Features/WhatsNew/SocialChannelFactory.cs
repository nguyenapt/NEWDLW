using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class SocialChannelFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new[]
            {
                new SelectItem() { Text =SocialChannel.Facebook.ToString() , Value = SocialChannel.Facebook },
                new SelectItem() { Text =SocialChannel.Twitter.ToString() , Value = SocialChannel.Twitter },
                new SelectItem() { Text =SocialChannel.Instagram.ToString() , Value = SocialChannel.Instagram },
                new SelectItem() { Text =SocialChannel.Youtube.ToString() , Value = SocialChannel.Youtube },
                new SelectItem() { Text =SocialChannel.LinkedIn.ToString() , Value = SocialChannel.LinkedIn },
            };
        }
    }
}