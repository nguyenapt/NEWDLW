using System.Web.Mvc;
using Dlw.EpiBase.Content.Infrastructure.Seo;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Dlw.EpiBase.Content.Infrastructure.Mvc
{
    public abstract class AbstractWebViewPage<TModel> : WebViewPage<TModel>
    {
        private readonly IContentRouteHelper _contentRouteHelper;
        private readonly ISeoSettings _seoSettings;

        public string PageTitle { get; set; }

        public IContent Content { get; set; }

        public AbstractWebViewPage()
        {
            // DI doesn't work in this context
            _contentRouteHelper = ServiceLocator.Current.GetInstance<IContentRouteHelper>();
            _seoSettings = ServiceLocator.Current.GetInstance<ISeoSettings>();
        }

        protected override void InitializePage()
        {
            // NEXT render -context aware- default page title
            var seoInformation = _contentRouteHelper.Content as ISeoInformation;
            if (seoInformation != null && !string.IsNullOrWhiteSpace(seoInformation.SeoPageTitle))
            {
                PageTitle = $"{seoInformation.SeoPageTitle}{_seoSettings.TitleSuffix}";
            }
            else
            {
                PageTitle = $"{_contentRouteHelper.Content?.Name}{_seoSettings.TitleSuffix}";
            }

            Content = _contentRouteHelper.Content;

            base.InitializePage();
        }
    }
}