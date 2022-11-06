using EPiServer.ServiceLocation;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Core.Extensions
{
    public static class CountryExtensions
    {
        public static string ToCountryName(this string code)
        {
            var countryRepository = ServiceLocator.Current.GetInstance<ICountryRepository>();
            return countryRepository.GetCountryNameByISOCode(code);
        }
    }
}