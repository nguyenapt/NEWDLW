using System;
using EPiServer;
using EPiServer.Construction;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Netafim.WebPlatform.Web.Core.Bynder
{
    public class DefaultAssetConverter : IAssetConverter
    {
        private readonly IAssetMappedIdentityBuilder _assetMappedIdentityBuilder;
        private readonly IUrlSegmentGenerator _urlSegmentGenerator;
        private readonly IContentFactory _contentFactory;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IBlobFactory _blobFactory;
        private readonly IContentProviderManager _providerManager;
        private readonly ContentMediaResolver _contentMediaResolver;

        private ContentReference _entryPoint;

        public DefaultAssetConverter(IAssetMappedIdentityBuilder assetMappedIdentityBuilder,
            IUrlSegmentGenerator urlSegmentGenerator,
            IContentFactory contentFactory,
            IContentTypeRepository contentTypeRepository,
            IBlobFactory blobFactory,
            IContentProviderManager providerManager,
            ContentMediaResolver contentMediaResolver)
        {
            _assetMappedIdentityBuilder = assetMappedIdentityBuilder;
            _urlSegmentGenerator = urlSegmentGenerator;
            _contentFactory = contentFactory;
            _contentTypeRepository = contentTypeRepository;
            _blobFactory = blobFactory;
            _providerManager = providerManager;
            _contentMediaResolver = contentMediaResolver;
        }

        private ContentReference GetEntryPoint()
        {
            // TODO add locking
            // TODO get node directly, like in initializer?

            if (_entryPoint != null) return _entryPoint;

            _entryPoint = _providerManager.GetProvider(BynderProvider.Key).EntryPoint;

            return _entryPoint;
        }

        public MediaData ConvertToContent(AssetData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var type = _contentTypeRepository.Load(ResolveContentType(data));

            var asset = _contentFactory.CreateContent(type, new BuildingContext(type)
            {
                Parent = DataFactory.Instance.Get<ContentFolder>(GetEntryPoint())
            }) as MediaData;

            asset.Status = VersionStatus.Published;
            asset.IsPendingPublish = false;

            asset.Changed = data.LastModified ?? data.Created;
            asset.StartPublish = data.Created;

            var mappedIdentity = _assetMappedIdentityBuilder.GetMappedIdentity(data.Id);

            asset.ContentTypeID = type.ID;
            asset.ContentLink = mappedIdentity.ContentLink;
            asset.ContentGuid = mappedIdentity.ContentGuid;

            asset.Name = data.Name;

            string filename = data.Name;
            if (!string.IsNullOrWhiteSpace(data.Extension))
            {
                filename = $"{data.Name}.{data.Extension}";
            }

            (asset as IRoutable).RouteSegment = $"{data.Id}-{_urlSegmentGenerator.Create(filename)}";

            var blob = BinaryDataToBlob(data.Id, ImageDerivative.Large, filename);
            asset.BinaryData = blob;

            var thumbnailBlob = BinaryDataToBlob(data.Id, ImageDerivative.Thumbnail, filename);
            asset.Thumbnail = thumbnailBlob;

            var securable = asset as IContentSecurable;
            securable.GetContentSecurityDescriptor().AddEntry(new AccessControlEntry(EveryoneRole.RoleName, AccessLevel.Read));

            var versionable = asset as IVersionable;
            if (versionable != null)
            {
                versionable.Status = VersionStatus.Published;
            }

            var changeTrackable = asset as IChangeTrackable;
            if (changeTrackable != null)
            {
                changeTrackable.Changed = DateTime.Now;
            }

            asset.MakeReadOnly();

            return asset;
        }

        public Type ResolveContentType(AssetData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            // TODO use _contentMediaResolver?

            return ResolveContentTypeInternal(data.Extension);
        }

        public Type ResolveContentType(AssetInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            return ResolveContentTypeInternal(info.Extension);
        }

        private Type ResolveContentTypeInternal(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case "jpg":
                case "jpeg":
                case "png":
                    return typeof(ImageAsset);
                case "mp4":
                case "mpeg":
                case "mpg":
                    return typeof(VideoAsset);
                default:
                    return null;
            }

        }

        private Blob BinaryDataToBlob(string assetId, string type, string filename)
        {
            // TODO review
            //Uri newBlobIdentifier = Blob.NewBlobIdentifier(contentMedia.BinaryDataContainer, localFile.Extension);
            //var dummy = new FileBlob(newBlobIdentifier, localFile.FullName);

            var uri = BlobProvider.BynderBlobProvider.GenerateBlobUri(assetId, type, filename);

            var blob = _blobFactory.GetBlob(uri); // TODO use CreateBlob?

            return blob;
        }
    }
}