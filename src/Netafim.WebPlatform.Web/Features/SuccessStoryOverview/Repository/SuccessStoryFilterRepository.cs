using System.Collections.Generic;
using System.Linq;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.CropsOverview;
using Netafim.WebPlatform.Web.Features.JobFilter;
using Netafim.WebPlatform.Web.Features.SuccessStory;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview.Repository
{
    public class SuccessStoryFilterRepository : ISuccessStoryFilterRepository
    {
        private readonly IPageService _pageService;
        private readonly IFindSettings _findSettings;

        public SuccessStoryFilterRepository(IPageService pageService, IFindSettings findSettings)
        {
            _pageService = pageService;
            _findSettings = findSettings;
        }

        public Dictionary<int, string> Crops()
        {
            var res = new Dictionary<int, string>();
            var results = GetSuccessStoryPages();
            if (!results.Any())
                return res;

            var cropPages = _pageService.GetPages<CropsPage>(_findSettings.MaxItemsPerRequest, m => m.MatchType(typeof(CropsPage)));
            foreach (var result in results.Where(t => t.CropId > 0))
            {
                var cropName = result.CropId.GetCropNameByCropId(cropPages);
                res.AddIfNotExist<int, string>(result.CropId, cropName);
            }
            return res;
        }

        public Dictionary<string, string> Countries()
        {
            var res = new Dictionary<string, string>();
            var results = GetSuccessStoryPages();
            if (!results.Any())
                return res;

            foreach (var result in results.Where(result => !string.IsNullOrEmpty(result.Country)))
            {
                res.AddIfNotExist(result.Country, result.Country.ToCountryName());
            }
            return res;
        }

        private IEnumerable<SuccessStoryPage> GetSuccessStoryPages()
        {
            var results = _pageService.GetPages<SuccessStoryPage>(_findSettings.MaxItemsPerRequest, m => m.MatchType(typeof(SuccessStoryPage)));
            return results;
        }
    }

    public interface ISuccessStoryFilterRepository
    {
        Dictionary<int, string> Crops();
        Dictionary<string, string> Countries();
    }
}