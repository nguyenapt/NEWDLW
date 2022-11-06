using EPiServer.Shell.ObjectEditing;
using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties
{
    public class JobLocation : IEquatable<JobLocation>
    {
        public JobLocation() { }
        public JobLocation(string name, string coutry)
        {
            LocationName = name;
            Country = coutry;
        }
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [AutoSuggestSelection(typeof(CountrySelectionQuery))]
        public string Country { get; set; }

        [Display(Name = "Country Name")]
        public string CountryName
        {
            get
            {
                var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
                return countryRepository.GetCountryNameByISOCode(Country);
            }
        }

        public bool Equals(JobLocation other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return Country.Refined().Equals(other.Country.Refined()) && LocationName.Refined().Equals(other.LocationName.Refined());
        }

        public override int GetHashCode()
        {
            int hashProductName = Country == null ? 0 : Country.Refined().GetHashCode();
            int hashProductCode = LocationName.Refined().GetHashCode();
            return hashProductName ^ hashProductCode;
        }
    }
}