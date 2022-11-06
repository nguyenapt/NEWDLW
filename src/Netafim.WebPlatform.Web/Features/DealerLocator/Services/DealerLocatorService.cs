using Dlw.EpiBase.Content.Cms;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using Netafim.WebPlatform.Web.Core.Templates;
using System;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.DealerLocator.Services
{
    public class DealerLocatorService : FindPageService, IDealerLocatorService
    {
        public DealerLocatorService(IClient searchClient, IUserContext userContext, IContentRepository contentRepository, IFindSettings findSettings) 
            : base(searchClient, userContext, contentRepository, findSettings)
        {
        }

        public IContentResult<ICanBeSearched> GetDealers(Expression<Func<ICanBeSearched, Filter>> filter, GeoLocation searchLocation = null)
        {
            var query = this.GetQuery<ICanBeSearched>(this.FindSettings.MaxItemsPerRequest, filter);

            query = searchLocation != null ? query.OrderBy(m => ((DealerLocatorPage)m).LocationForSearch).DistanceFrom(searchLocation) : query.OrderBy(m => m.Title);

            return query.GetContentResult();
        }
    }
}
