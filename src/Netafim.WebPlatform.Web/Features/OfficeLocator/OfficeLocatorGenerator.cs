using EPiServer.Shell.ViewComposition;
using EPiServer.Cms.Shell.UI.Rest.Internal;
using Netafim.WebPlatform.Web.Features.GeneralContactInfo;
using Dlw.EpiBase.Content.Infrastructure.Data.ContentGenerator;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Security;
using EPiServer.Web;
using Netafim.WebPlatform.Web.Features.Home;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.RichText.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Features.Settings;
using System.Globalization;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    public class OfficeLocatorGenerator : IContentGenerator
    {
        private const string DataDemoFolder = @"~/Features/RichText/Data/Demo/{0}";

        private readonly IContentRepository _contentRepository;
        private readonly IUrlSegmentCreator _urlSegmentCreator;
        private readonly ContentAssetHelper _contentAssetHelper;

        public OfficeLocatorGenerator(IContentRepository contentRepository, IUrlSegmentCreator urlSegmentCreator,
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
            officeLocatorPage.PageName = "Netafim Worldwide";
            
            var officePageContentAsset = this._contentAssetHelper.GetOrCreateAssetFolder(Save(officeLocatorPage));

            GenerateOffices();

            CreateNetafimWorldWideBlock(officeLocatorPage, officePageContentAsset.ContentLink);

            CreateGeneralContactInfoBlock(officeLocatorPage, officePageContentAsset.ContentLink);

            Save(officeLocatorPage);            
        }

        private void CreateGeneralContactInfoBlock(GenericContainerPage containerPage, ContentReference assetFolder)
        {
            var generalContacInfoContainer = this._contentRepository.GetDefault<GeneralContactInfoContainerBlock>(assetFolder);
            ((IContent)generalContacInfoContainer).Name = "Contact Info";
            generalContacInfoContainer.Title = "General contact info";
            generalContacInfoContainer.Watermark = "General";

            Save((IContent)generalContacInfoContainer);

            generalContacInfoContainer.Items = generalContacInfoContainer.Items ?? new ContentArea();

            generalContacInfoContainer.Items.Items.Add(new ContentAreaItem() { ContentLink = CreateContactInfoItem(assetFolder) });
            generalContacInfoContainer.Items.Items.Add(new ContentAreaItem() { ContentLink = CreateContactInfoItem(assetFolder) });
            generalContacInfoContainer.Items.Items.Add(new ContentAreaItem() { ContentLink = CreateContactInfoItem(assetFolder) });

            containerPage.Content = containerPage.Content ?? new ContentArea();
            containerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)generalContacInfoContainer) });
        }

        private void CreateNetafimWorldWideBlock(GenericContainerPage containerPage, ContentReference assetFolder)
        {
            var item = this._contentRepository.GetDefault<OfficeLocatorBlock>(assetFolder);
            ((IContent)item).Name = "Netafim worldwide";

            containerPage.Content = containerPage.Content ?? new ContentArea();

            containerPage.Content.Items.Add(new ContentAreaItem() { ContentLink = Save((IContent)item) });
        }
        
        private IEnumerable<Tuple<double, double>> RandomOfficeCoordination(int totalOffices)
        {
            Random rng = new Random();

            for (int i = 0; i < totalOffices; i++)
            {
                int lat = rng.Next(40, 70);
                int lng = rng.Next(0, 50);

                yield return new Tuple<double, double>(lat, lng);
            }
        }

        private void GenerateOffices()
        {
            var officeContainer = this._contentRepository.GetDefault<OfficeLocatorContainerPage>(ContentReference.RootPage);
            officeContainer.PageName = "Office container";

            Save(officeContainer);

            GenerateOfficeLocatorPages(officeContainer.ContentLink);

            var container = Save(officeContainer);

            AddSettings(container);
        }

        private void AddSettings(ContentReference officeContainer)
        {
            var settingPage = EnsureSettingsPage().CreateWritableClone() as SettingsPage;
            settingPage.OfficeContainer = officeContainer;
            settingPage.Radius = 200;
            settingPage.GoogleMapsApiKey = "AIzaSyBe1MQGr0CBlJPqUxGpWtg5Rg0uOri_fzg";
            

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

        private void GenerateOfficeLocatorPages(ContentReference containerPage)
        {
            var conidations = RandomOfficeCoordination(100).ToList();
            var countries = ListCountries();

            for (var i = 0; i < conidations.Count; i ++)
            {
                var office = this._contentRepository.GetDefault<OfficeLocatorPage>(containerPage);

                var countryIndex = new Random().Next(0, countries.Count);

                office.PageName = "NETAFIM SWEDEN " + i;
                office.OfficeName = "NETAFIM SWEDEN " + i;
                office.Address = "5470 East Home Avenue Fresno, CA 93727";
                office.Phone = "1 559 453 6800";
                office.Fax = "1 559 453 6800";
                office.Email = "contact@netafim.com";
                office.Website = "netafim.com";
                office.Direction = @"https://www.google.com/maps/place/6%2F231+George+St,+Brisbane+City+QLD+4000,+Australia/@-27.4705414,153.0212438,17z/data=!3m1!4b1!4m5!3m4!1s0x6b915a0458dce2a7:0x3b1e56d74b694899!8m2!3d-27.4705414!4d153.0234325";
                office.Longtitude = conidations[i].Item2;
                office.Latitude = conidations[i].Item1;
                office.Country = countries.Keys.ElementAt(countryIndex);                

                Save(office);
            }
        }

        private Dictionary<string,string> ListCountries()
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

        private ContentReference CreateContactInfoItem(ContentReference assetFolder)
        {
            var item = this._contentRepository.GetDefault<GeneralContactInfoItemBlock>(assetFolder);
            ((IContent)item).Name = "Contact Item";
            item.Title = "CORPORATE HEADQUARTERS";
            item.Information = new XhtmlString("<p>Netafim Ltd.</p><p>Derech Hashlom 10</p><p>Tel Aviv, Israel 67892</p></div>");
            item.Phone = "972 - 8 - 6474747";
            item.Fax = "972 - 8 - 6474747";
            item.MailTo = "contact@netafim.com";
            item.LinkTextButton = "CONTACT HELENE";

            return Save((IContent)item);
        }
                 
        private ContentReference Save(IContent content)
        {
            return this._contentRepository.Save(content, SaveAction.Publish, AccessLevel.NoAccess);
        }
    }
}