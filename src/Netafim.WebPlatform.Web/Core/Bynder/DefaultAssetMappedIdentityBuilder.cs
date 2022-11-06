using System;
using EPiServer.DataAbstraction;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public class DefaultAssetMappedIdentityBuilder : IAssetMappedIdentityBuilder
    {
        private readonly IdentityMappingService _identityMappingService;

        public DefaultAssetMappedIdentityBuilder(IdentityMappingService identityMappingService)
        {
            _identityMappingService = identityMappingService;
        }

        public MappedIdentity GetMappedIdentity(string assetId)
        {
            if (string.IsNullOrWhiteSpace(assetId)) throw new ArgumentNullException(nameof(assetId));

            // generate external identifer to create local content link
            var constructExternalIdentifier = MappedIdentity.ConstructExternalIdentifier(BynderProvider.Key, assetId);

            // generate content link
            var mappedIdentity = _identityMappingService.Get(constructExternalIdentifier, true);

            return mappedIdentity;
        }
    }
}