using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using EPiServer.Core;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Mappers;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository
{
    public class DefaultSystemConfiguratorRepository : ISystemConfiguratorRepository
    {
        protected readonly SystemConfiguratorDbContext DbContext;

        public DefaultSystemConfiguratorRepository(SystemConfiguratorDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Delete(CultureInfo culture)
        {
            Delete<DecisionTreeEntity>(culture);

            Delete<RegionEntity>(culture);
            Delete<CropEntity>(culture);

            Delete<WaterSourceEntity>(culture);
            Delete<FiltrationTypeEntity>(culture);
            
            Delete<ProductEntity>(culture);
            Delete<ContentEntity>(culture);
            Delete<ContactEntity>(culture);

            DbContext.SaveChanges();
        }

        public void Store<T>(IEnumerable<T> entities) where T : SystemConfiguratorEntityBase
        {
            DbContext.Set<T>().AddRange(entities);
            DbContext.SaveChanges();
        }

        public IEnumerable<Crop> GetAllCrops(CultureInfo culture)
        {
            return Find<CropEntity>(x => x.Culture == culture.Name).Map();
        }

        public IEnumerable<Region> GetAllRegions(CultureInfo culture)
        {
            return Find<RegionEntity>(x => x.Culture == culture.Name).Map();
        }

        public IEnumerable<FiltrationType> GetAllFiltrationTypes(CultureInfo culture)
        {
            return Find<FiltrationTypeEntity>(x => x.Culture == culture.Name).Map();
        }

        public IEnumerable<WaterSource> GetAllWaterSources(CultureInfo culture)
        {
            return Find<WaterSourceEntity>(x => x.Culture == culture.Name).Map();
        }

        public IEnumerable<Contact> GetContacts(int cropId, int regionId, CultureInfo culture)
        {
            var contacts = DbContext.DecisionTrees.Where(e => (e.Crop == null || e.Crop.Id == cropId) && (e.Region == null || e.Region.Id == regionId) && e.Culture == culture.Name)
                .Include(x => x.Entity)
                .Where(e => e.Entity is ContactEntity)
                .Select(x => x.Entity);

            var mappedContacts = new List<Contact>();

            foreach (var contact in contacts)
            {
                var mappedContact = MapContact(contact);
                if(contact != null) mappedContacts.Add(mappedContact);
            }

            return mappedContacts;
        }

        public IEnumerable<Dealer> GetDealers(int cropId, int regionId, CultureInfo culture)
        {
            var dealers = DbContext.DecisionTrees.Where(e => (e.Crop == null || e.Crop.Id == cropId) && (e.Region == null || e.Region.Id == regionId) && e.Culture == culture.Name)
                .Include(x => x.Entity)
                .Where(e => e.Entity is DealerEntity)
                .Select(x => x.Entity);

            var mappedDealers = new List<Dealer>();

            foreach (var dealer in dealers)
            {
                var mappedDealer = MapContent(dealer);
                if (mappedDealer != null) mappedDealers.Add(mappedDealer as Dealer);
            }

            return mappedDealers;
        }

        public Crop GetCrop(int cropId)
        {
            var entity = FindOne<CropEntity>(x => x.Id == cropId);
            if (entity == null) return null;

            return new Crop
            {
                Id = entity.Id,
                Name = entity.Name,
                CropFactor = entity.CropFactor
            };
        }

        public Region GetRegion(int regionId)
        {
            var entity = FindOne<RegionEntity>(x => x.Id == regionId);
            if (entity == null) return null;

            return new Region
            {
                Id = entity.Id,
                Name = entity.Name,
                Dealer = entity.Dealer,
                Eto = entity.Eto,
                DealerPhone = entity.DealerPhone,
                NetafimPhone = entity.NetafimPhone,
                NetafimSales = entity.NetafimSales
            };
        }

        public FiltrationType GetFiltrationType(int filtrationTypeId)
        {
            var entity = FindOne<FiltrationTypeEntity>(x => x.Id == filtrationTypeId);
            if (entity == null) return null;

            return new FiltrationType {Id = entity.Id, Name = entity.Name};
        }

        public WaterSource GetWaterSource(int waterSourceId)
        {
            var entity = FindOne<WaterSourceEntity>(x => x.Id == waterSourceId);
            if (entity == null) return null;

            return new WaterSource {Id = entity.Id, Name = entity.Name};
        }

        public IEnumerable<Product> GetProducts(int cropId, int regionId, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            // TODO remove workaround, EF seems to have caching issue, can not load nested entities
            var watersources = DbContext.WaterSources.ToList();
            var filtrationTypes = DbContext.FiltrationTypes.ToList();

            var products = DbContext.DecisionTrees.Where(e => (e.Crop == null || e.Crop.Id == cropId) && (e.Region == null || e.Region.Id == regionId) && e.Culture == culture.Name)
                .Include(x => x.Entity)
                .Where(e => e.Entity is ProductEntity)
                .Select(x => x.Entity);

            var mappedProducts = new List<Product>();

            foreach (var product in products)
            {
                var mappedProduct = MapProduct(product);
                if (mappedProduct != null) mappedProducts.Add(mappedProduct);
            }

            return mappedProducts;
        }

        public IEnumerable<Content> GetContent(int cropId, int regionId, CultureInfo culture) 
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            var contents = DbContext.DecisionTrees.Where(e => (e.Crop == null || e.Crop.Id == cropId) && (e.Region == null || e.Region.Id == regionId) && e.Culture == culture.Name)
                            .Include(x => x.Entity)
                            .Where(e => e.Entity is ContentEntity)
                            .Select(x => x.Entity);

            var mappedContents = new List<Content>();

            foreach (var content in contents)
            {
                var mappedContent = MapContent(content);
                if(mappedContent != null) mappedContents.Add(mappedContent);
            }

            return mappedContents;
        }

        #region Private

        private Content MapContent(SystemConfiguratorEntityBase content)
        {
            var digitalFarming = content as DigitalFarmingEntity;
            if (digitalFarming != null)
            {
                return new DigitalFarming
                {
                    ContentPage = new ContentReference(digitalFarming.ContentReference)
                };
            }

            var solution = content as SolutionEntity;
            if (solution != null)
            {
                return new Solution
                {
                    ContentPage = new ContentReference(solution.ContentReference)
                };
            }

            var dealer = content as DealerEntity;
            if (dealer != null)
            {
                return new Dealer
                {
                    ContentPage = new ContentReference(dealer.ContentReference)
                };
            }

            return null;
        }

        private Product MapProduct(SystemConfiguratorEntityBase product)
        {
            var connector = product as ConnectorEntity;
            if (connector != null)
            {
                return new Connector
                {
                    CatalogNumber = connector.CatalogNumber,
                    ContentPage = new ContentReference(connector.ContentReference),
                    Name = connector.Name,
                    Id = connector.Id
                };
            }

            var filtration = product as FiltrationEntity;
            if (filtration != null)
            {
                return new Filtration
                {
                    CatalogNumber = filtration.CatalogNumber,
                    ContentPage = new ContentReference(filtration.ContentReference),
                    Name = filtration.Name,
                    Id = filtration.Id,
                    FlowRate = filtration.FlowRate,
                    FiltrationType = MapFiltrationType(filtration.FiltrationType),
                    WaterSource = MapWaterSource(filtration.WaterSource)
                };
            }

            var dripperLine = product as DripperLineEntity;
            if (dripperLine != null)
            {
                return new Dripperline
                {
                    CatalogNumber = dripperLine.CatalogNumber,
                    ContentPage = new ContentReference(dripperLine.ContentReference),
                    Name = dripperLine.Name,
                    Id = dripperLine.Id,
                    EmiterSpacing = dripperLine.EmiterSpacing,
                    NumberOfLaterals = dripperLine.NumberOfLaterals,
                    FlowRate = dripperLine.FlowRate,
                    FlowVariation = dripperLine.FlowVariation
                };
            }

            var pipe = product as PipeEntity;
            if (pipe != null)
            {
                return new Pipe
                {
                    CatalogNumber = pipe.CatalogNumber,
                    Name = pipe.Name,
                    ContentPage = new ContentReference(pipe.ContentReference),
                    Id = pipe.Id
                };
            }

            var valve = product as ValveEntity;
            if (valve != null)
            {
                return new Valve
                {
                    CatalogNumber = valve.CatalogNumber,
                    ContentPage = new ContentReference(valve.ContentReference),
                    Name = valve.Name,
                    Id = valve.Id
                };
            }

            return null;
        }

        private WaterSource MapWaterSource(WaterSourceEntity filtrationWaterSource)
        {
            return new WaterSource()
            {
                Id =  filtrationWaterSource.Id,
                Name = filtrationWaterSource.Name
            };
        }

        private FiltrationType MapFiltrationType(FiltrationTypeEntity filtrationFiltrationType)
        {
            return new FiltrationType()
            {
                Id = filtrationFiltrationType.Id,
                Name = filtrationFiltrationType.Name
            };
        }

        private Contact MapContact(SystemConfiguratorEntityBase contact)
        {
            var contactEntity = contact as ContactEntity;
            if (contactEntity == null) return null;
            return new Contact
            {
                FirstName = contactEntity.FirstName,
                LastName = contactEntity.LastName,
                PhoneNumber = contactEntity.PhoneNumber
            };
        }

        private Region MapRegion(RegionEntity region)
        {
            if (region == null) return null;
            return new Region
            {
                Dealer = region.Dealer,
                Name = region.Name,
                DealerPhone = region.DealerPhone,
                NetafimPhone = region.NetafimPhone,
                Id = region.Id,
                Eto = region.Eto,
                NetafimSales = region.NetafimSales
            };
        }

        private void Delete<T>(CultureInfo culture) where T: SystemConfiguratorEntityBase
        {
            Delete(Find<T>(entity => entity.Culture == culture.Name));
        }

        private void Delete<T>(IEnumerable<T> entities) where T : SystemConfiguratorEntityBase
        {
            this.DbContext.Set<T>().RemoveRange(entities);
        }

        private IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicated) where T : SystemConfiguratorEntityBase
        {
            return this.DbContext.Set<T>().Where(predicated);
        }

        private T FindOne<T>(Expression<Func<T, bool>> predicated) where T : SystemConfiguratorEntityBase
        {
            return this.DbContext.Set<T>().FirstOrDefault(predicated);
        }

        private T Get<T>(object key) where T : SystemConfiguratorEntityBase
        {
            return this.DbContext.Set<T>().Find(key);
        }

        #endregion
    }
}
