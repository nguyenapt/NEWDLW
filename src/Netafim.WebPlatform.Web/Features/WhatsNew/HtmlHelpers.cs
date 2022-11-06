using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    public static class HtmlHelpers
    {
        public static string RenderSocialConnectorClass(this HtmlHelper helper, SocialChannel channel)
        {
            switch (channel)
            {
                case SocialChannel.Facebook: return "dlw-icon-facebook";
                case SocialChannel.Instagram: return "dlw-icon-instagram";
                case SocialChannel.LinkedIn: return "dlw-icon-linkedin";
                case SocialChannel.Youtube: return "dlw-icon-youtube";
                case SocialChannel.Twitter: return "dlw-icon-twitter";
                default: return string.Empty;
            }
        }
        public static string RenderFeedItemClass(this HtmlHelper helper, SocialChannel channel)
        {
            switch (channel)
            {
                case SocialChannel.Facebook: return "from-facebook";
                case SocialChannel.Instagram: return "from-instagram";
                case SocialChannel.LinkedIn: return "from-linkedin";
                case SocialChannel.Youtube: return "from-youtube";
                case SocialChannel.Twitter: return "from-twitter";
                default: return string.Empty;
            }
        }
    }
}