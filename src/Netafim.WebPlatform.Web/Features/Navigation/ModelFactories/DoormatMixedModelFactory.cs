using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Navigation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using EPiServer.DataAbstraction;

namespace Netafim.WebPlatform.Web.Features.Navigation.ModelFactories
{
    public class DoormatMixedModelFactory : DoormatModelFactory
    {
        private readonly int MaxSupportedLevelForImageColumn = 1;
        private readonly int MaxSupportedLevelForTextColumn = 3;
        private readonly int MaxImageItems = 3;

        public DoormatMixedModelFactory(IContentRepository contentRepo, IPageService pageService, IContentTypeRepository contentTypeRepo) : base(contentRepo, pageService, contentTypeRepo)
        {
        }

        public override IEnumerable<DoormatNavigationItemModel> Create(NavigationPage navPage, ContentReference currentLink, bool excludeInvisible)
        {
            var result = base.Create(navPage, currentLink, excludeInvisible).ToList();
            return ReOrder(result);
        }
        public override bool IsSatisfied(NavigationPage navPage)
        {
            return navPage != null && (DoormatType)navPage.DoormatType == DoormatType.MixedImageAndTextColumn;
        }

        protected override int GetMaxLevelSupported(INavigationItem currentNode)
        {
            if (!ContentReference.IsNullOrEmpty(currentNode.NavigationImageLink)) return MaxSupportedLevelForImageColumn;
            return MaxSupportedLevelForTextColumn;
        }

        protected IEnumerable<DoormatNavigationItemModel> ReOrder(List<DoormatNavigationItemModel> originalList)
        {
            if (originalList.IsNullOrEmpty()) return originalList;
            var navItemsWithImage = originalList.Where(x => !ContentReference.IsNullOrEmpty(x.ImageLink));
            if (!navItemsWithImage.IsNullOrEmpty())
            {
                navItemsWithImage = navItemsWithImage.Take(MaxImageItems);
            }
            var navItemsWithoutImage = originalList.Where(x => ContentReference.IsNullOrEmpty(x.ImageLink));

            return Enumerable.Union(navItemsWithImage, navItemsWithoutImage);
        }
    }
}