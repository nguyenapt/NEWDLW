using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Features.WhatsNew.ModelFactories;
using Netafim.WebPlatform.Web.Features.WhatsNew.SocialServices;

namespace Netafim.WebPlatform.Web.Features.WhatsNew
{
    [InitializableModule]
    public class IocConfig : IConfigurableModule
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(IocConfig));

        public void Initialize(InitializationEngine context)
        {
            // do nothing
        }

        public void Uninitialize(InitializationEngine context)
        {
            // do nothing
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<ISocialChannelFeedFactory, FacebookFeedFactory>();
            context.Services.AddTransient<ISocialChannelFeedFactory, TwitterFeedFactory>();
            context.Services.AddTransient<ISocialChannelFeedFactory, LinkedInFeedFactory>();
            context.Services.AddTransient<ISocialChannelFeedFactory, YoutubeFeedFactory>();
            context.Services.AddTransient<ISocialChannelFeedFactory, InstagramFeedFactory>();
            context.Services.AddSingleton<IFacebookAppSettings, ConfigurationManagerSocialAppSettings>();
            context.Services.AddSingleton<ILinkedInAppSettings, ConfigurationManagerSocialAppSettings>();
            context.Services.AddSingleton<ITwitterAppSettings, ConfigurationManagerSocialAppSettings>();
            context.Services.AddSingleton<IYouTubeAppSettings, ConfigurationManagerSocialAppSettings>();

            context.Services.AddTransient<ISocialService, FacebookService>();
            context.Services.AddTransient<ISocialService, TwitterService>();
            context.Services.AddTransient<ISocialService, InstagramService>();
            context.Services.AddTransient<ISocialService, YoutubeService>();
            context.Services.AddTransient<ISocialService, LinkedInService>();

        }
    }
}