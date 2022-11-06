using System.Collections.Generic;
using System.Linq;
using EPiServer.Cms.Shell.Search;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Localization;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Search;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    [SearchProvider]
    public class BynderContentSearchProvider : ContentSearchProviderBase<MediaData, ContentType>
    {
        private readonly IBynderRepository _bynderRepository;
        private readonly IAssetConverter _assetConverter;

        public override string Area => BynderProvider.Key;

        public override string Category => BynderProvider.Key;

        protected override string IconCssClass => "epi-resourceIcon epi-resourceIcon-block";

        public BynderContentSearchProvider(IBynderRepository bynderRepository,
            IAssetConverter assetConverter,
            LocalizationService localizationService,
            ISiteDefinitionResolver siteDefinitionResolver,
            IContentTypeRepository<ContentType> contentTypeRepository,
            EditUrlResolver editUrlResolver,
            ServiceAccessor<SiteDefinition> currentSiteDefinition,
            LanguageResolver languageResolver,
            UrlResolver urlResolver,
            TemplateResolver templateResolver,
            UIDescriptorRegistry uiDescriptorRegistry) : base(localizationService, siteDefinitionResolver, contentTypeRepository, editUrlResolver, currentSiteDefinition, languageResolver, urlResolver, templateResolver, uiDescriptorRegistry)
        {
            _bynderRepository = bynderRepository;
            _assetConverter = assetConverter;
        }

        public override IEnumerable<SearchResult> Search(Query query)
        {
            var results = _bynderRepository.FindAssets(query.SearchQuery);

            foreach (var assetInfo in results)
            {
                var contentType = _assetConverter.ResolveContentType(assetInfo);

                if (contentType == null) continue; // file not supported

                var data = _bynderRepository.GetAsset(assetInfo.Id);

                yield return CreateSearchResult(_assetConverter.ConvertToContent(data));
            }
        }
    }
}