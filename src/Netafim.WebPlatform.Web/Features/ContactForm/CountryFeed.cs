using System.Collections.Generic;
using System.Linq;
using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Feed.Internal;
using EPiServer.Forms.EditView.Models.Internal;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Features.ContactForm
{
    /// <summary>
    /// link to refer: https://www.epinova.no/en/blog/custom-data-feeds-in-episerver.forms/
    /// </summary>
    [ServiceConfiguration(ServiceType = typeof(IFeed))]
    public class CountryFeed : IFeed, IUIEntityInEditView
    {
        public IEnumerable<IFeedItem> LoadItems()
        {
            var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
            return countryRepository.GetCountries().OrderBy(p => p.Value)
                .Select(t => new FeedItem
                {
                    Key = t.Value,
                    Value = t.Value
                });
        }

        public string ID => "d4d5d468-0993-403a-9485-56dd7f8a9dcc";

        public string Description { get { return "Country Feed"; } set { } }

        public string ExtraConfiguration => string.Empty;

        public string EditViewFriendlyTitle => Description;

        public bool AvailableInEditView => true;
    }
}