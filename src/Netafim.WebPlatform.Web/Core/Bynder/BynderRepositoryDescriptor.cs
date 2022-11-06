using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    [ServiceConfiguration(typeof(IContentRepositoryDescriptor))]
    public class BynderRepositoryDescriptor : ContentRepositoryDescriptorBase
    {
        private readonly IContentProviderManager _providerManager;
        private readonly IBynderSettings _settings;

        public BynderRepositoryDescriptor(IContentProviderManager providerManager, IBynderSettings settings)
        {
            _providerManager = providerManager;
            _settings = settings;
        }

        public override string SearchArea => BynderProvider.Key;

        public override string Key => BynderProvider.Key;

        public override string Name => _settings.ProviderName;

        public override IEnumerable<ContentReference> Roots
        {
            get
            {
                if (!_settings.Enabled) return null;

                return new[] { _providerManager.GetProvider(BynderProvider.Key).EntryPoint };
            }
        }

        public override IEnumerable<Type> ContainedTypes
        {
            get { return new[] { typeof(ImageAsset), typeof(VideoAsset) }; }
        }

        public override IEnumerable<Type> MainNavigationTypes
        {
            get { return new[] { typeof(ContentFolder) }; }
        }
    }
}