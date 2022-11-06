using Castle.Core.Internal;
using Netafim.WebPlatform.Web.Features.JobFilter.CustomProperties;
using System.Collections.Generic;
using System.Linq;
using Netafim.WebPlatform.Web.Core.Repository;

namespace Netafim.WebPlatform.Web.Features.JobFilter
{
    public class JobRepository : IJobRepository
    {
        private readonly IJobFilterSettings _jobFilterSettings;
        private readonly ICountryRepository _countryRepository;
        public JobRepository(IJobFilterSettings jobFilterSettings, ICountryRepository countryRepository)
        {
            _jobFilterSettings = jobFilterSettings;
            _countryRepository = countryRepository;
        }
        public Dictionary<string, string> GetAllDepartments()
        {
            var result = new Dictionary<string, string>();
            var allDepartments = _jobFilterSettings.JobDepartments;
            if (allDepartments.IsNullOrEmpty()) { return result; }
            allDepartments = allDepartments.OrderBy(dp => dp.DepartmentName).ToList();
            foreach (var department in allDepartments)
            {
                result.AddIfNotExist(ToKey(department.DepartmentName), department.DepartmentName);
            }
            return result;
        }

        public Dictionary<string, string> GetAllLocations()
        {
            var result = new Dictionary<string, string>();
            var allLocations = _jobFilterSettings.JobLocations;
            if (allLocations.IsNullOrEmpty()) { return result; }
            var countryRepeatations = allLocations
                                        .Distinct()                
                                        .GroupBy(x => x.Country)
                                        .Select(x => new { Country = x.Key, Repeat = x.Count() > 1 })
                                        .ToDictionary(t => t.Country, t => t.Repeat);
            countryRepeatations = countryRepeatations ?? new Dictionary<string, bool>();
            var fullLocationNames = new List<string>();
            foreach (var location in allLocations)
            {
                if (string.IsNullOrWhiteSpace(location.Country)) { continue; }
                fullLocationNames.Add(ExtractFullLocationName(location, countryRepeatations));                
            }
            fullLocationNames = fullLocationNames.OrderBy(x => x).ToList();

            foreach (var fullLocationName in fullLocationNames)
            {
                var countryName = _countryRepository.GetCountryNameByISOCode(fullLocationName);
                result.AddIfNotExist(fullLocationName, countryName);
            }
            
            return result;
        }

        public Dictionary<string, string> GetAllPositions()
        {
            var result = new Dictionary<string, string>();
            var allPositions = _jobFilterSettings.JobPositions;
            if (allPositions.IsNullOrEmpty()) { return result; }
            foreach (var position in allPositions)
            {
                result.AddIfNotExist(ToKey(position.JobName), position.JobName);
            }
            return result;
        }

        public JobDepartment GetDepartment(string key)
        {
            return _jobFilterSettings.JobDepartments.FirstOrDefault(s => ToKey(s.DepartmentName) == ToKey(key));
        }

        public JobPosition GetPosition(string key)
        {
            return _jobFilterSettings.JobPositions.FirstOrDefault(s => ToKey(s.JobName) == ToKey(key));
        }

        private string ToKey(string @source) => !string.IsNullOrWhiteSpace(source)
            ? source.Replace('&', '_')
                    .Replace('!', '_')
                    .Replace('%', '_')
                    .Trim()
            : source;

        private string ExtractFullLocationName(JobLocation location, Dictionary<string, bool> countryRepeatations)
        {
            var country = location.Country;
            if (countryRepeatations.ContainsKey(country) && countryRepeatations[country] && !string.IsNullOrWhiteSpace(location.LocationName))
            {
                return string.Format("{0} ({1})", location.Country, location.LocationName);
            }
            return location.Country;
        }
    }
}