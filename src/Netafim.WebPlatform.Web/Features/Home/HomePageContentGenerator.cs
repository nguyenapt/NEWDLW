using System;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;

namespace Netafim.WebPlatform.Web.Features.Home
{
    public class HomePageContentGenerator : IContentGenerator
    {
        private IContentRepository _contentRepository;
        private IUrlSegmentCreator _urlSegmentCreator;

        public HomePageContentGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator)
        {
            _contentRepository = contentRepository;
            _urlSegmentCreator = urlSegmentCreator;
        }

        public void Generate(ContentContext context)
        {
            // TODO add content to HomePage.Content property.
        }
    }
}