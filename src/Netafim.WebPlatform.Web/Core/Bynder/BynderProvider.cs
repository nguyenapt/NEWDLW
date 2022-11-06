using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    // commerce -> mediachase.commerce / business.commerce
    // storage provider / blobprovder
    // sources:
    // - http://slides.com/im5tu/epi-content-providers
    // - https://github.com/episerver/YouTubeContentProvider
    // - commerce manager catalog content provider

    public class BynderProvider : ContentProvider
    {
        public const string Key = "bynder";

        private readonly IBynderRepository _bynderRepository;
        private readonly IdentityMappingService _identityMappingService;
        private readonly IAssetMappedIdentityBuilder _assetMappedIdentityBuilder;
        private readonly IAssetConverter _assetConverter;

        public BynderProvider(IBynderRepository bynderRepository,
            IdentityMappingService identityMappingService,
            IAssetMappedIdentityBuilder assetMappedIdentityBuilder,
            IAssetConverter assetConverter)
        {
            _bynderRepository = bynderRepository;
            _identityMappingService = identityMappingService;
            _assetMappedIdentityBuilder = assetMappedIdentityBuilder;
            _assetConverter = assetConverter;
        }

        public override string ProviderKey => Key;

        protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
        {
            // map content link to external id known @ bynder
            var mappedIdentity = _identityMappingService.Get(contentLink);

            // ignore root node
            if (mappedIdentity == null) return null;

            // id is uri, extract bynder id
            string id = mappedIdentity.ExternalIdentifier.Segments[1];

            return _assetConverter.ConvertToContent(_bynderRepository.GetAsset(id));
        }

        // return all used bynder images in epi until resolving is fixed
        protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(ContentReference contentLink, string languageID, out bool languageSpecific)
        {
            // the assets are not language specific, so this is ignored.
            languageSpecific = false;

            var items = new List<GetChildrenReferenceResult>();

            if (contentLink.CompareToIgnoreWorkID(EntryPoint))
            {
                // TODO return collections as folders

                foreach (var asset in _bynderRepository.GetAssets())
                {
                    var contentType = _assetConverter.ResolveContentType(asset);

                    if (contentType == null) continue; // file not supported

                    var item = new GetChildrenReferenceResult()
                    {
                        ContentLink = _assetMappedIdentityBuilder.GetMappedIdentity(asset.Id).ContentLink,
                        ModelType = contentType,
                        IsLeafNode = true
                    };

                    items.Add(item);
                }

                return items;
            }

            // TODO return content collection-folders

            return new List<GetChildrenReferenceResult>() { new GetChildrenReferenceResult()
            {
                ContentLink = contentLink,
                ModelType = typeof(MediaData),
                IsLeafNode = true
            } };
        }

        protected override IList<MatchingSegmentResult> ListMatchingSegments(ContentReference parentLink, string urlSegment)
        {
            // TODO disable loading all children?
            // TODO investigate why I never come into this function anymore

            var assetId = urlSegment.Substring(0, 35);
            var mappedIdentity = _assetMappedIdentityBuilder.GetMappedIdentity(assetId);

            return new List<MatchingSegmentResult>() { new MatchingSegmentResult()
            {
                ContentLink = mappedIdentity.ContentLink
            }};
        }
    }
}