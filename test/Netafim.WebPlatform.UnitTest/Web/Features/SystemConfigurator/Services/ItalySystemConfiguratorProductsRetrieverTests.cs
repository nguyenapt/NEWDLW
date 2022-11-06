using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.UnitTest.Web.Features.SystemConfigurator.Services
{
    [TestClass]
    public class ItalySystemConfiguratorProductsRetrieverTests
    {
        /*
            scenario 1

            input:
                Location: EMILIA ROMAGNA (From database "city" column A "Region")
                Crop: OLIVO (From database crops column "B")
                Plot area size: 20 ha 
                Row spacing: 3 m
                Max. allowed irrigation time per day: 10 hrs
                Weekly irrigation interval: 7 day
                Filtration type (Automatic or manual): AUTOMATIC
                Water source (well or surface water): SURFACE WATER
            output: 67
        */

        [TestMethod]
        public void Scenario_1_returns_relevant_products_for_specific_set_of_data()
        {
            var crop = new Crop()
            {
                Id = 1,
                CropFactor = 0.5m
            };

            var region = new Region()
            {
                Id = 2,
                Eto = 6m
            };

            Scenario_1(crop, region, Scenario_1_get_products());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Scenario_1_throws_exception_when_data_is_not_valid()
        {
            var crop = new Crop()
            {
                Id = -1,
                CropFactor = 1000m
            };

            var region = new Region()
            {
                Id = 2,
                Eto = -100m
            };

            Scenario_1(crop, region, Scenario_1_get_products());
        }

        private void Scenario_1(Crop crop, Region region, IEnumerable<Product> products)
        {
            // arrange
            var repository = new Mock<ISystemConfiguratorRepository>();
            var calculator = new Mock<ISystemConfiguratorFlowRateCalculator>();
            var culture = CultureInfo.CreateSpecificCulture("en");

            repository.Setup(x => x.GetCrop(It.IsAny<int>())).Returns(crop);
            repository.Setup(x => x.GetRegion(It.IsAny<int>())).Returns(region);
            repository.Setup(x => x.GetFiltrationType(It.IsAny<int>())).Returns(new FiltrationType() { Id = 1, Name = "manual" });
            repository.Setup(x => x.GetWaterSource(It.IsAny<int>())).Returns(new WaterSource() { Id = 2, Name = "well" });
            repository.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(products);
            calculator.Setup(x => x.Calculate(It.IsAny<SystemConfiguratorFlowRateData>())).Returns(67);

            var data = new SystemConfiguratorData()
            {
                CropId = crop.Id,
                RegionId = region.Id,
                WeeklyIrrigationInterval = 7,
                RowSpacing = 3,
                MaxAllowedIrrigationTimePerDay = 10,
                PlotArea = 20,
                FiltrationTypeId = 1,
                WaterSourceId = 2
            };

            // act
            var service = new ItalySystemConfiguratorProductsRetriever(repository.Object, calculator.Object);
            IEnumerable<Product> result = service.GetProducts(data, culture);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(x => x.CatalogNumber == "70605-010820")); 
            Assert.IsTrue(result.Count(x => x.GetType() == typeof(Filtration)) == 1);
            Assert.IsTrue(result.Count(x => x.GetType() == typeof(Connector)) == 1);
            Assert.IsTrue(result.Count(x => x.GetType() == typeof(Dripperline)) == 1);
            Assert.IsTrue(result.Count(x => x.GetType() == typeof(Pipe)) == 1);
                          
            Assert.IsTrue(result.Count(x => x.GetType() == typeof(Valve)) == 2);
            
            repository.Verify(x => x.GetCrop(It.IsAny<int>()), Times.Once);
            repository.Verify(x => x.GetRegion(It.Is<int>(id => id == 2)), Times.Once);
            repository.Verify(x => x.GetFiltrationType(It.Is<int>(id => id == 1)), Times.Once);
            repository.Verify(x => x.GetWaterSource(It.Is<int>(id => id == 2)), Times.Once);
            repository.Verify(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>()), Times.Once);
            calculator.Verify(x => x.Calculate(It.IsAny<SystemConfiguratorFlowRateData>()), Times.Once);
        }

        private IEnumerable<Product> Scenario_1_get_products()
        {
            var products = new List<Product>();
            products.Add(new Filtration
            {
                Name = "Filtration 1",
                ContentPage = null,
                CatalogNumber = "70605-######",
                FlowRate = 476,
                FiltrationType = new FiltrationType() { Id = 1, Name = "manual" },
                WaterSource = new WaterSource() { Id = 2, Name = "well" }
            });

            products.Add(new Filtration
            {
                Name = "Filtration 2",
                ContentPage = null,
                CatalogNumber = "70605-010820",
                FlowRate = 78,
                FiltrationType = new FiltrationType() { Id = 1, Name = "manual" },
                WaterSource = new WaterSource() { Id = 2, Name = "well" }
            });

            products.Add(new Filtration
            {
                Name = "Filtration 3",
                ContentPage = null,
                CatalogNumber = "70605-######",
                FlowRate = 104,
                FiltrationType = new FiltrationType() { Id = 1, Name = "manual" },
                WaterSource = new WaterSource() { Id = 2, Name = "well" }
            });

            products.Add(new Filtration
            {
                Name = "Filtration 4",
                ContentPage = null,
                CatalogNumber = "70605-######",
                FlowRate = 65,
                FiltrationType = new FiltrationType() { Id = 1, Name = "manual" },
                WaterSource = new WaterSource() { Id = 2, Name = "well" }
            });

            products.Add(new Connector
            {
                Name = "Connector",
                ContentPage = null,
                CatalogNumber = "70605-000001"
            });

            products.Add(new Dripperline
            {
                Name = "Dripperline",
                ContentPage = null,
                CatalogNumber = "70605-000002"
            });

            products.Add(new Pipe
            {
                Name = "Flextnet",
                ContentPage = null,
                CatalogNumber = "70605-000003"
            });

            products.Add(new Valve
            {
                Name = "Valve",
                ContentPage = null,
                CatalogNumber = "70605-000004"
            });

            products.Add(new Valve
            {
                Name = "Air valve",
                ContentPage = null,
                CatalogNumber = "70605-00005"
            });

            return products;
        }
    }
}