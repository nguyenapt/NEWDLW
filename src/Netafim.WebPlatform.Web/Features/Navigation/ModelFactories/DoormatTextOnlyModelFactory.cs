using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.DataAbstraction;
using Netafim.WebPlatform.Web.Core.Templates;

namespace Netafim.WebPlatform.Web.Features.Navigation.ModelFactories
{
    public class DoormatTextOnlyModelFactory : DoormatModelFactory
    {
        private readonly int MaxSupportedLevel = 2;

        public DoormatTextOnlyModelFactory(IContentRepository contentRepo, IPageService pageService, IContentTypeRepository contentTypeRepo) : base(contentRepo, pageService, contentTypeRepo)
        {
        }

        public override bool IsSatisfied(NavigationPage navPage)
        {
            return navPage != null && (DoormatType)navPage.DoormatType == DoormatType.TextColumnOnly;
        }

        protected override int GetMaxLevelSupported(INavigationItem currentNode)
        {
            return MaxSupportedLevel;
        }
    }
}