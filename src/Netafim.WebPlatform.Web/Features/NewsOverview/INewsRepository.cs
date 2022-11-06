using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.NewsOverview
{
    public interface INewsRepository
    {
        int GetLatestYearHavingNews(IEnumerable<int> yearsRange);
        IEnumerable<NewsPage> GetLatestNewsPages(int maximumNewsToDisplay);
    }
}
