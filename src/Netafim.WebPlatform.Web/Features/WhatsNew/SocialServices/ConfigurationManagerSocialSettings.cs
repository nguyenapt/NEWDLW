using Dlw.EpiBase.Content.Infrastructure;

namespace Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices
{
    public class ConfigurationManagerSocialAppSettings : BaseConfigurationManagerSettings, IFacebookAppSettings,
        ILinkedInAppSettings, ITwitterAppSettings, IYouTubeAppSettings
    {
        public string FacebookAppId => GetAppSetting<string>("Social.FacebookAppId");

        public string FacebookAppSecret => GetAppSetting<string>("Social.FacebookAppSecret");

        public string FacebookName => GetAppSetting<string>("Social.FbName");

        public string LinkedInAppId => GetAppSetting<string>("Social.LinkedInAppId");

        public string LinkedInAppSecret => GetAppSetting<string>("Social.LinkedInAppSecret");

        public string LinkedInAppName => GetAppSetting<string>("Social.LinkedInAppName");

        public int LinkedInTokenExpired => GetAppSetting<int>("Social.LinkedInTokenExpired");

        public int LinkedInFeedExpired => GetAppSetting<int>("Social.LinkedInFeedExpired");

        public string LinkedInAuthorizedUrl => GetAppSetting<string>("Social.LinkedInAuthorizedUrl");

        public string TwitterConsumerKey => GetAppSetting<string>("Social.TwitterConsumerKey");

        public string TwitterConsumerSecret => GetAppSetting<string>("Social.TwitterConsumerSecret");

        public string TwitterName => GetAppSetting<string>("Social.TwitterName");

        public string YouTubeApiKey => GetAppSetting<string>("YouTube.ApiKey");
    }
    public interface IFacebookAppSettings
    {
        string FacebookAppId { get; }
        string FacebookAppSecret { get; }       
        string FacebookName { get; }
    }
    public interface ITwitterAppSettings
    {
        string TwitterConsumerKey { get; }
        string TwitterConsumerSecret { get; }
        string TwitterName { get; }
    }

    public interface ILinkedInAppSettings
    {
        string LinkedInAppId { get; }
        string LinkedInAppSecret { get; }
        string LinkedInAppName { get; }
        string LinkedInAuthorizedUrl { get; }
        int LinkedInTokenExpired { get; }
        int LinkedInFeedExpired { get; }
    }

    public interface IYouTubeAppSettings
    {
        string YouTubeApiKey { get; }
    }
}