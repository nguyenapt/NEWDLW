using System;
using System.Globalization;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Localization.Internal;
using EPiServer.ServiceLocation;

// workaround for epi / DbLocalizationProvider issue
// https://github.com/valdisiljuconoks/LocalizationProvider/issues/41

namespace Dlw.EpiBase.Content.Infrastructure.Localization
{
    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization))] // used to override default provider
    public class CustomProviderBasedLocalizationService : ProviderBasedLocalizationService, IConfigurableModule
    {
        private static LocalizationService _localizationService;

        public CustomProviderBasedLocalizationService() : this(null) { }

        public CustomProviderBasedLocalizationService(ResourceKeyHandler keyHandler) : base(keyHandler) { }

        protected override string LoadString(string[] normalizedKey, string originalKey, CultureInfo culture)
        {
            var list = Providers.ToList(); // important fix for collection was modified issue

            foreach (LocalizationProvider provider in list)
            {
                string str = provider.GetString(originalKey, normalizedKey, culture);

                if (str != null)
                    return str;
            }
            return null;
        }

        /// <summary>
        /// Used to create service container instance
        /// </summary>
        /// <param name="keyHandler"></param>
        /// <param name="localizationOptions"></param>
        /// <param name="serviceLocator"></param>
        /// <returns></returns>
        public static LocalizationService CreateCustomProviderBasedLocalizationService(ResourceKeyHandler keyHandler, LocalizationOptions localizationOptions, IServiceLocator serviceLocator)
        {
            if (_localizationService == null)
            {
                ProviderBasedLocalizationService localizationService = new CustomProviderBasedLocalizationService(keyHandler);

                foreach (Func<IServiceLocator, LocalizationProvider> providerFactory in localizationOptions.ProviderFactories)
                {
                    localizationService.Providers.Add(providerFactory(serviceLocator));
                }

                localizationService.FallbackBehavior = localizationOptions.FallbackBehavior;
                localizationService.FallbackCulture = localizationOptions.FallbackCulture;

                _localizationService = localizationService;
            }

            return _localizationService;
        }

        public new void ConfigureContainer(ServiceConfigurationContext context)
        {
            // register as a factory
            context.Services.AddSingleton(typeof(LocalizationService), (locator) =>
                CreateCustomProviderBasedLocalizationService(locator.GetInstance<ResourceKeyHandler>(), locator.GetInstance<LocalizationOptions>(), locator));
        }
    }
}