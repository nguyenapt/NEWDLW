using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Hosting;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Dlw.EpiBase.Content.Infrastructure.Mvc;
using Netafim.WebPlatform.Web.Core.Rendering;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class IContentExtensions
    {
        public static ContentReference CreateBlob(this IContent contentData, string fileName)
        {
            var contentMediaResolver = ServiceLocator.Current.GetInstance<ContentMediaResolver>();
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var blockFactory = ServiceLocator.Current.GetInstance<IBlobFactory>();

            if (!File.Exists(HostingEnvironment.MapPath(fileName)))
            {
                return ContentReference.EmptyReference;
            }

            var fileInfo = new FileInfo(HostingEnvironment.MapPath(fileName));

            var mediaType = contentMediaResolver.GetFirstMatching(fileInfo.Extension);
            var contentType = contentTypeRepository.Load(mediaType);
            if (contentType == null)
                return ContentReference.EmptyReference;

            var contentFolder = GetOrCreateComponentFolder(contentData, contentRepository);
            var media = contentRepository.GetDefault<MediaData>(contentFolder, contentType.ID);
            media.Name = fileInfo.Name;

            var blob = blockFactory.CreateBlob(media.BinaryDataContainer, fileInfo.Extension);
            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                blob.Write(fs);
            }

            media.BinaryData = blob;

            return contentRepository.Save(media, SaveAction.Publish, AccessLevel.NoAccess);
        }

        public static string GetDefaultViewName(this IContentData content)
        {
            var contentType = content.GetOriginalType();
            return $"_{contentType.Name.Substring(0, 1).ToLowerInvariant()}{contentType.Name.Substring(1)}";
        }

        public static string GetDefaultFullViewName(this IContentData content)
        {
            var blockType = content.GetOriginalType(); // use OriginalType of proxy
            
            // find feature
            var featureName = blockType.Namespace.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();

            return $"{FeatureRazorViewEngine.FeatureLocation}/{featureName}/Views/{content.GetDefaultViewName()}.cshtml";
        }

        /// <summary>
        /// Extensions method to verify the property can be saved to the database.
        /// </summary>
        /// <param name="contentData"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static bool PropertyCanBeSaved(this IContentData contentData, Expression<Func<IContentData, object>> propertyExpression)
        {
            if (contentData == null)
                throw new ArgumentNullException(nameof(contentData));

            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException(nameof(propertyExpression), "The expression is not a member expression");

            var property = contentData.GetOriginalType().GetProperty(memberExpression.Member.Name);

            if (property == null)
                throw new Exception($"The type {contentData.GetOriginalType().Name} doesn't have property {memberExpression.Member.Name}");

            var ignoreAttribute = property.GetCustomAttribute<IgnoreAttribute>();

            return ignoreAttribute == null;
        }

        private static ContentReference GetOrCreateComponentFolder(this IContent contentData, IContentRepository contentRepository)
        {
            const string mediaGeneratorFolderName = "Demo";

            var demoFolder = contentRepository.GetChildren<ContentFolder>(SiteDefinition.Current.GlobalAssetsRoot)
                .FirstOrDefault(x => x.Name == mediaGeneratorFolderName);

            ContentReference demoContentLink;
            if (demoFolder != null)
            {
                demoContentLink = demoFolder.ContentLink;
            }
            else
            {
                demoFolder = contentRepository.GetDefault<ContentFolder>(SiteDefinition.Current.GlobalAssetsRoot);
                demoFolder.Name = mediaGeneratorFolderName;
                demoContentLink = contentRepository.Save(demoFolder, SaveAction.Publish, AccessLevel.NoAccess);
            }
            if (string.IsNullOrEmpty(contentData?.Name))
            {
                return demoContentLink;
            }

            var featureFolder = contentRepository.GetChildren<ContentFolder>(demoContentLink)
                .FirstOrDefault(x => x.Name == contentData.Name);

            if (featureFolder != null)
            {
                return featureFolder.ContentLink;
            }

            featureFolder = contentRepository.GetDefault<ContentFolder>(demoContentLink);
            featureFolder.Name = contentData.Name;

            return contentRepository.Save(featureFolder, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}