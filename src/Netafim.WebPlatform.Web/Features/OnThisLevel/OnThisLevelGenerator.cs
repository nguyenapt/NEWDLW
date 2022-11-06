using System;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.OnThisLevel
{
    public class OnThisLevelGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly ContentAssetHelper _contentAssetHelper;
        private readonly IUrlSegmentCreator _urlSegmentCreator;

        public OnThisLevelGenerator(
            IContentRepository contentRepository, 
            IUrlSegmentCreator urlSegmentCreator, 
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var onThisLevelPageRef = EnsurePage(context);

            var onThisLevelBlockRef = EnsureBlock(onThisLevelPageRef);

            var onthisLevelPage = _contentRepository.Get<GenericContainerPage>(onThisLevelPageRef).CreateWritableClone() as GenericContainerPage;

            if (onthisLevelPage == null) throw new Exception($"Could not find newly created page of type ${typeof(GenericContainerPage)}.");

            if (onthisLevelPage.Content == null)
            {
                onthisLevelPage.Content = new ContentArea();
            }

            onthisLevelPage.Content.Items.Add(new ContentAreaItem
            {
                ContentLink = onThisLevelBlockRef
            });

            _contentRepository.Save(onthisLevelPage, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference EnsurePage(ContentContext context)
        {
            var onThisLevelPage = _contentRepository.GetDefault<GenericContainerPage>(context.Homepage);

            onThisLevelPage.PageName = "On this level";
            onThisLevelPage.URLSegment = _urlSegmentCreator.Create(onThisLevelPage);
            onThisLevelPage.Title = "On this level";
            onThisLevelPage.Description = new XhtmlString(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas vitae pretium neque. Nam lorem dui, convallis in risus bibendum, vehicula eleifend sapien. Ut id lectus tristique, fringilla mauris in, porta nisl. Cras sed libero sit amet metus varius euismod in in ligula. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Proin quis nibh lectus.");

            return _contentRepository.Save(onThisLevelPage, SaveAction.Publish, AccessLevel.NoAccess);
        }

        private ContentReference EnsureBlock(ContentReference onThisLevelRef)
        {
            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(onThisLevelRef);

            var block = _contentRepository.GetDefault<OnThisLevelBlock>(assetsFolder.ContentLink);
            ((IContent)block).Name = "onThisLevelBlock";
            block.Title = "Other categories";
            block.SubTitle = "Other categories";
            block.Watermark = "Categories";

            return _contentRepository.Save((IContent)block, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}