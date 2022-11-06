using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netafim.WebPlatform.Web.Core.Repository
{
    public interface ICountryRepository
    {
        Dictionary<string, string> GetCountries();
        string GetCountryNameByISOCode(string code);
    }

    public class CountryRepository : ICountryRepository
    {
        public Dictionary<string, string> GetCountries()
        {
            var countries = new Dictionary<string, string>();
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                      .Select(cultureInfo => new RegionInfo(cultureInfo.Name));

            foreach (var regionInfo in cultures.Where(regionInfo => !countries.ContainsKey(regionInfo.TwoLetterISORegionName)))
            {
                countries.Add(regionInfo.TwoLetterISORegionName, regionInfo.EnglishName);
            }

            return countries;
        }

        public string GetCountryNameByISOCode(string code)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                      .Select(cultureInfo => new RegionInfo(cultureInfo.Name));
            var country = cultures.FirstOrDefault(t => t.TwoLetterISORegionName.Equals(code, StringComparison.OrdinalIgnoreCase));
            return country?.EnglishName ?? code;
        }
    }
}
