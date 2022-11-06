using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public class DealerContentGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/RichText/Data/Demo/{0}";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public DealerContentGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
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
            var officeLocatorPage = this._contentRepository.GetDefault<GenericContainerPage>(context.Homepage);
            officeLocatorPage.PageName = "Dealer Locator";

            var officePageContentAsset = this._contentAssetHelper.GetOrCreateAssetFolder(Save(officeLocatorPage));

            GenerateDealers();

            CreateDealerLocatorBlock(officeLocatorPage, officePageContentAsset.ContentLink);
            
            Save(officeLocatorPage);
        }
        
        private void CreateDealerLocatorBlock(GenericContainerPage containerPage, ContentReference assetFolder)
        {
            var item = this._contentRepository.GetDefault<DealerLocatorBlock>(assetFolder);
            ((IContent)item).Name = "Dealer Locator";

            containerPage.Content = containerPage.Content ?? new ContentArea();

            containerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)item) });
        }

        private IEnumerable<Tuple<double, double>> RandomOfficeCoordination(int totalDealers)
        {
            Random rng = new Random();

            for (int i = 0; i < totalDealers; i++)
            {
                int lat = rng.Next(40, 70);
                int lng = rng.Next(0, 50);

                yield return new Tuple<double, double>(lat, lng);
            }
        }

        private void GenerateDealers()
        {
            var dealerContainer = this._contentRepository.GetDefault<NoTemplateContainerPage>(ContentReference.RootPage);
            dealerContainer.PageName = "Dealer Container";

            var containerReference = Save(dealerContainer);

            GenerateDealerGroup(containerReference, DealerLevel.Partner, DealerColor.Blue);
            GenerateDealerGroup(containerReference, DealerLevel.Secondary, DealerColor.Orange);
            GenerateDealerGroup(containerReference, DealerLevel.Thirdary, DealerColor.DarkBlue);

            AddSettings(containerReference);
        }

        private ContentReference GenerateDealerGroup(ContentReference containerReference, DealerLevel level, DealerColor color)
        {
            var dealerLocator = this._contentRepository.GetDefault<DealerLocatorContainerPage>(containerReference);
            dealerLocator.PageName = "Dealer group";
            dealerLocator.Level = level;
            dealerLocator.Color = color;

            Save(dealerLocator);

            GenerateDealerLocatorPages(dealerLocator.ContentLink);

            return Save(dealerLocator);
        }

        private void AddSettings(ContentReference dealerContainer)
        {
            var settingPage = EnsureSettingsPage().CreateWritableClone() as SettingsPage;
            settingPage.DealerContainer = dealerContainer;
            settingPage.DealerRadius = 200;
            settingPage.GoogleMapsApiKey = "AIzaSyBe1MQGr0CBlJPqUxGpWtg5Rg0uOri_fzg";
            settingPage.MinimalDealersWhenSearchByLocation = 10;


            Save(settingPage);
        }

        private SettingsPage EnsureSettingsPage()
        {
            var settingsPage = _contentRepository.GetChildren<SettingsPage>(ContentReference.RootPage).SingleOrDefault();
            if (settingsPage != null) return settingsPage;

            var page = _contentRepository.GetDefault<SettingsPage>(ContentReference.RootPage);
            if (_contentRepository.Save(page, SaveAction.Publish, AccessLevel.NoAccess) != null) { return page; };

            return null;
        }

        private void GenerateDealerLocatorPages(ContentReference containerPage)
        {
            var conidations = RandomOfficeCoordination(50).ToList();
            for (var i = 0; i < conidations.Count; i++)
            {
                var office = this._contentRepository.GetDefault<DealerLocatorPage>(containerPage);

                office.PageName = "NETAFIM SWEDEN " + i;
                office.DealerName = "NETAFIM SWEDEN " + i;
                office.Address = "5470 East Home Avenue Fresno, CA 93727";
                office.Phone = "1 559 453 6800";
                office.Email = "contact@netafim.com";
                office.Website = "netafim.com";
                office.Direction = @"https://www.google.com/maps/place/6%2F231+George+St,+Brisbane+City+QLD+4000,+Australia/@-27.4705414,153.0212438,17z/data=!3m1!4b1!4m5!3m4!1s0x6b915a0458dce2a7:0x3b1e56d74b694899!8m2!3d-27.4705414!4d153.0234325";
                office.Longtitude = (double)conidations[i].Item2;
                office.Latitude = (double)conidations[i].Item1;

                Save(office);
            }
        }
        
        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}