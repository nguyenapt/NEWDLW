using Dlw.EpiBase.Content.Cms.Search;
using Dlw.EpiBase.Content.Cms;
using Dlw.EpiBase.Content.Convertors;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Dlw.EpiBase.Content
{
    [InitializableModule]
    public class IocConfig : IConfigurableModule
    {
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
            context.Services.AddHttpContextScoped<IUserContext, DefaultUserContext>();
            context.Services.AddHttpContextScoped<IShellContext, ShellContext>();

            context.Services.AddSingleton<IRegionCodeConvertor, CulturesRegionCodeConvertor>();
            context.Services.AddSingleton<IExtendedUrlResolver, DefaultExtendedUrlResolver>();

            context.Services.AddTransient<IFindSettings, ConfigurationManagerFindSettings>();
            context.Services.AddTransient<IPageService, FindPageService>();
        }
    }
}