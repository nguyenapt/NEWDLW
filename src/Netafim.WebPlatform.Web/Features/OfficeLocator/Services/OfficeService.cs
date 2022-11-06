using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Cms;
using EPiServer.Find;
using Dlw.EpiBase.Content.Cms.Search;
using System;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator.Services
{
    public class OfficeService : IOfficeService
    {
        protected readonly IPageService PageService;
        protected readonly IOfficeSettings OfficeSettings;
        protected readonly IGoogleSettings GoogleSettings;
        protected readonly IClient Client;

        private const int MaximumItemsPerPage = 1000;

        public OfficeService(IPageService pageService,
            IOfficeSettings officeSettings,
            IGoogleSettings googleSettings,
            IClient client)
        {
            this.PageService = pageService;
            this.OfficeSettings = officeSettings;
            this.GoogleSettings = googleSettings;
            this.Client = client;
        }

        public IEnumerable<OfficeLocatorPage> Search(string countryCode, double? longtitude, double? latitude)
        {
            var filter = GetFilter(countryCode, longtitude, latitude);

            return PageService.GetPages(MaximumItemsPerPage, filter.Expression);
        }

        public IEnumerable<OfficeLocatorPage> Search()
        {
            if (OfficeSettings?.OfficeContainer == null)
                throw new Exception("The office container root is not set. Please add the setting for office search");

            return PageService.GetPages<OfficeLocatorPage>(MaximumItemsPerPage, m => m.ParentLink.ID.Match(OfficeSettings.OfficeContainer.ID));
        }

        private FilterExpression<OfficeLocatorPage> GetFilter(string countryCode, double? longtitude, double? latitude)
        {
            var officeContainer = OfficeSettings?.OfficeContainer;

            var filterBuilder = this.Client.BuildFilter<OfficeLocatorPage>();
            
            if (officeContainer == null)
                throw new Exception("The office container root is not set. Please add the setting for office search");

            if(longtitude.HasValue && latitude.HasValue)
            {
                var userLocation = new GeoLocation(latitude.Value, longtitude.Value);

                filterBuilder = filterBuilder.And(m => m.LocationForSearch.WithinDistanceFrom(userLocation, OfficeSettings.Radius.Kilometers()));
            }

            if(!string.IsNullOrWhiteSpace(countryCode))
            {
                filterBuilder = filterBuilder.Or(m => m.Country.Match(countryCode));
            }

            filterBuilder = filterBuilder.And(m => m.ParentLink.ID.Match(officeContainer.ID));

            return new FilterExpression<OfficeLocatorPage>(m => filterBuilder);
        }
    }
}
