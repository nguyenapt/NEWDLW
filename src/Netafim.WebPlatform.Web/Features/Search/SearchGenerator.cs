using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;

namespace Netafim.WebPlatform.Web.Features.Search
{
    public class SearchGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;

        public SearchGenerator(
            IContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        public void Generate(ContentContext context)
        {
            var searchPage = _contentRepository.GetDefault<SearchPage>(context.Homepage);
            searchPage.PageName = "Search page";
            searchPage.Title = "Search";
            searchPage.Watermark = "Search the site";
            _contentRepository.Save(searchPage, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
        }
    }
}