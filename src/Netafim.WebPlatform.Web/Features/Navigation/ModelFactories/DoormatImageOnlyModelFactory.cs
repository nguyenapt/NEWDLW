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
    public class DoormatImageOnlyModelFactory : DoormatModelFactory
    {
        private readonly int MaxLevelSupported = 1;
        private readonly int MaxItemsToDisplay = 4;

        public DoormatImageOnlyModelFactory(IContentRepository contentRepo, IPageService pageService, IContentTypeRepository contentTypeRepo) : base(contentRepo, pageService, contentTypeRepo)
        {
        }
        public override IEnumerable<DoormatNavigationItemModel> Create(NavigationPage navPage, ContentReference currentLink, bool excludeInvisible)
        {
            var result = base.Create(navPage, currentLink, excludeInvisible).ToList();
            if (!result.IsNullOrEmpty())
            {
                return result.Take(MaxItemsToDisplay);
            }
            return result;
        }

        public override bool IsSatisfied(NavigationPage navPage)
        {
            return navPage != null && (DoormatType)navPage.DoormatType == DoormatType.ImageColumnOnly;
        }

        protected override int GetMaxLevelSupported(INavigationItem currentNode)
        {
            return MaxLevelSupported;
        }
    }
}