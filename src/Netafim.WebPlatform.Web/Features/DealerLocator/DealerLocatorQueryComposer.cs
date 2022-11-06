using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Api;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public class DealerLocatorQueryComposer : QueryComposerBase<DealerLocatorBlock, DealerLocatorQueryViewModel>
    {
        protected readonly IDealerSettings DealerSettings;
        protected readonly IClient Client;

        public DealerLocatorQueryComposer(IContentLoader contentLoader,
            IDealerSettings dealerSettings,
            IClient client)
            : base(contentLoader)
        {
            this.DealerSettings = dealerSettings;
            this.Client = client;
        }

        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            var dealerFilter = this.Client.BuildFilter<ICanBeSearched>();
            var dealerQuery = this.GetQueryModel(query);

            var dealerGroup = DealerSettings?.DealerContainer;

            if (dealerGroup == null)
                throw new Exception("The dealer container root is not set. Please add the setting for dealer search");

            var dealerContainers = this.ContentLoader.GetChildren<DealerLocatorContainerPage>(dealerGroup);

            // Only search by geolocation when input have value.
            if (dealerQuery.HasGeoLocation() && dealerQuery.HasSearchText())
            {
                var userLocation = new GeoLocation(dealerQuery.Latitude.Value, dealerQuery.Longtitude.Value);

                var radiusSearch = dealerQuery.RadiusSearch > 0 ? dealerQuery.RadiusSearch : DealerSettings.DealerRadius;

                dealerFilter = dealerFilter.Or(m => ((DealerLocatorPage)m).LocationForSearch.WithinDistanceFrom(userLocation, radiusSearch.Kilometers()));
            }

            if (dealerQuery.HasSearchText())
            {
                dealerFilter = dealerFilter.Or(m => m.Title.Wildcard(dealerQuery.SearchText))
                                           .Or(m => m.Summary.Wildcard(dealerQuery.SearchText));
            }

            dealerFilter = dealerFilter.And(m => m.MatchType(typeof(DealerLocatorPage)));

            if (dealerContainers != null && dealerContainers.Any())
            {
                var containerIds = dealerContainers.Select(d => d.ContentLink.ID);

                dealerFilter = dealerFilter.And(m => ((DealerLocatorPage)m).ParentLink.ID.In(containerIds));
            }

            return new FilterExpression<ICanBeSearched>(m => dealerFilter);
        }        
    }
}