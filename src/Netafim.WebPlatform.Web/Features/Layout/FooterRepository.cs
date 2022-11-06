using EPiServer;
using Netafim.WebPlatform.Web.Features.Navigation;
using EPiServer.Core;
using System.Collections.Generic;
using System.Linq;

namespace Netafim.WebPlatform.Web.Features.Layout
{

    public class FooterRepository : IFooterRepository
    {
        private readonly INavigationSettings _navigationSettings;
        private readonly IContentLoader _contentLoader;

        public FooterRepository(INavigationSettings navigationSettings, IContentLoader contentLoader)
        {
            this._navigationSettings = navigationSettings;
            this._contentLoader = contentLoader;
        }

        public IEnumerable<NavigationPage> GetInternalLeadings()
        {
            var navigationRoot = _navigationSettings.NavigationRoot;

            if (ContentReference.IsNullOrEmpty(navigationRoot)) return Enumerable.Empty<NavigationPage>();

            NavigationContainerPage navContainerPage;
            if (!_contentLoader.TryGet(navigationRoot, out navContainerPage))
            {
                return Enumerable.Empty<NavigationPage>();
            }

            return _contentLoader.GetChildren<NavigationPage>(navigationRoot).FilterForDisplay(false);
        }
    }
}
