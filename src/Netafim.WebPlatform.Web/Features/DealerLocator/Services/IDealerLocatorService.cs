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
    public interface IDealerLocatorService : IPageService
    {
        IContentResult<ICanBeSearched> GetDealers(Expression<Func<ICanBeSearched, Filter>> filter, GeoLocation searchLocation = null);
    }
}
