using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.Weather
{
    public class WeatherGenerator : IContentGenerator
    {
        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public WeatherGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
            ContentAssetHelper contentAssetHelper)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
            _contentAssetHelper = contentAssetHelper;
        }

        public void Generate(ContentContext context)
        {
            EnsureComponent(context);
        }

        private void EnsureComponent(ContentContext context)
        {
            var genericContainerPage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            genericContainerPage.PageName = "Weather page";
            
            var blockReference = CreateWeatherBlock(Save(genericContainerPage));

            genericContainerPage.Content = new ContentArea();
            genericContainerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = blockReference });

            Save(genericContainerPage);
        }

        private ContentReference CreateWeatherBlock(ContentReference containerPageReference)
        {
            var assetsFolder = _contentAssetHelper.GetOrCreateAssetFolder(containerPageReference);
            var weatherBlock = _contentRepository.GetDefault<WeatherBlock>(assetsFolder.ContentLink);

            ((IContent)weatherBlock).Name = "Weather block";
            weatherBlock.DisplayFloating = true;
            weatherBlock.PaddingTop = 160;

            return Save((IContent)weatherBlock);
        }

        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}