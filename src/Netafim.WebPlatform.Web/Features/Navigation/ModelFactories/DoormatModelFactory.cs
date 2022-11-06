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
    public abstract class DoormatModelFactory : IDoormatModelFactory
    {
        protected readonly IContentRepository _contentRepo;
        protected readonly IPageService _pageService;
        protected readonly IContentTypeRepository _contentTypeRepo;

        public DoormatModelFactory(IContentRepository contentRepo, IPageService pageService, IContentTypeRepository contentTypeRepo)
        {
            _contentRepo = contentRepo;
            _pageService = pageService;
            _contentTypeRepo = contentTypeRepo;
        }

        public virtual IEnumerable<DoormatNavigationItemModel> Create(NavigationPage navPage, ContentReference currentLink, bool excludeInvisible)
        {
            var emptyResult = new List<DoormatNavigationItemModel>();
            var navChildren = GetNavChildren(navPage.Link);
            var children = navChildren.FilterForDisplay(false, excludeInvisible);
            if (children.IsNullOrEmpty()) return emptyResult;

            return children.OfType<INavigationItem>().Select(x => MapToDoormatItemModel(x.NavigationLink, currentLink, GetMaxLevelSupported(x))).ToList();
        }

        private IEnumerable<PageData> GetNavChildren(ContentReference link)
        {
            return _contentRepo.GetChildren<INavigationItem>(link).OfType<PageData>().FilterForDisplay(false, true).Select(x => x);
        }

        private DoormatNavigationItemModel MapToDoormatItemModel(ContentReference linkedPageRef, ContentReference curPageLink, int childLevel)
        {
            if (ContentReference.IsNullOrEmpty(linkedPageRef)) return null;
            var linkedPage = _contentRepo.Get<PageData>(linkedPageRef) as INavigationItem;
            var model = new DoormatNavigationItemModel((PageData)linkedPage);
            if (linkedPage == null) return model;

            model.Title = linkedPage.NavigationTitle;
            model.Link = linkedPage.NavigationLink;
            model.ImageLink = linkedPage.NavigationImageLink;
            model.IsActive = linkedPage.NavigationLink.IsActiveNavigationNode(curPageLink);
            var children = GetNavChildren(linkedPage.NavigationLink);
            childLevel--;
            if (!children.IsNullOrEmpty() && childLevel > 0)
            {
                model.Children = children.Select(item => MapToDoormatItemModel(item.ContentLink, curPageLink, childLevel)).ToList();
            }
            return model;
        }

        protected abstract int GetMaxLevelSupported(INavigationItem currentNode);

        public abstract bool IsSatisfied(NavigationPage navPage);
    }
}