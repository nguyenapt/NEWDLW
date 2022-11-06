using System.Reflection;
using EPiServer.Find.Helpers.Reflection;
using EPiServer.Find.Helpers;
using Netafim.WebPlatform.Web.Core.Templates;
using EPiServer.DataAbstraction;
using Antlr.Runtime.Misc;
using EPiServer.Find.Cms;
using EPiServer.Core;
using System.Linq;
using Netafim.WebPlatform.Web.Core;
using EPiServer.Find;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using System.Web.Mvc;

namespace Netafim.WebPlatform.Web.Features.GenericListing
{
    public class GenericListingController : BlockController<GenericListingBlock>
    {
        protected readonly IPageRouteHelper PageRouteHelper;
        protected readonly IContentLoader ContentLoader;
        protected readonly IPageService SearchService;
        protected readonly IFindSettings FindSettings;
        protected readonly IContentTypeRepository ContentTypeRepository;

        public GenericListingController(IContentLoader contentLoader, 
            IPageRouteHelper pageRouteHelper, IPageService searchService,
            IFindSettings findSettings, IContentTypeRepository contentTypeRepository)
        {
            this.ContentLoader = contentLoader;
            this.PageRouteHelper = pageRouteHelper;
            this.SearchService = searchService;
            this.FindSettings = findSettings;
            this.ContentTypeRepository = contentTypeRepository;
        }

        public override ActionResult Index(GenericListingBlock currentContent)
        {           
            var query = GetFilter(currentContent);

            var result = this.SearchService.GetPages(FindSettings.MaxItemsPerRequest, query.Expression)
                                        .OrderBy(m => m.Title);

            return PartialView("_listingContent", new GenericListingViewModel(currentContent, result.OfType<IPreviewable>()));
        } 
        
        private FilterExpression<Core.Templates.PageBase> GetFilter(GenericListingBlock currentContent)
        {
            var rootSearchLink = !ContentReference.IsNullOrEmpty(currentContent.SearchRoot)
                                 ? currentContent.SearchRoot
                                 : PageRouteHelper.PageLink;

            return new FilterExpression<Core.Templates.PageBase>(m => m.ParentLink.ID.Match(rootSearchLink.ID));
        }
    }
}