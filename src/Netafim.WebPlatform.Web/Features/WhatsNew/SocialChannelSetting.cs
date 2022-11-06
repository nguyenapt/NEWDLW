using EPiServer.Shell.ObjectEditing;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public class SocialChannelSetting
    {
        [SelectOne(SelectionFactoryType = typeof(SocialChannelFactory))]
        public SocialChannel Channel { get; set; }

        [Display(Name = "Number of Feeds")]
        [Range(0,5)]
        public int NumberOfFeeds { get; set; }
    }
}