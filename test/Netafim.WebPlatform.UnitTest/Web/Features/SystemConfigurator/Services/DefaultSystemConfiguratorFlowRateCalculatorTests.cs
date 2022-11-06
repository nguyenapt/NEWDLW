using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.UnitTest.Web.Features.SystemConfigurator.Services
{
    [TestClass]
    public class DefaultSystemConfiguratorFlowRateCalculatorTests
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
        public void Scenario_1_return_result_for_valid_data()
        {
            // arrange
            var systemConfiguratorRepository = new Mock<ISystemConfiguratorRepository>();
            systemConfiguratorRepository
                .Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>()))
                .Returns(new List<Product>
                {
                    new Dripperline()
                    {
                        FlowRate = 1.6m,
                        NumberOfLaterals = 1,
                        EmiterSpacing = 0.8m
                    }
                });

            var calculator = new ItalySystemConfiguratorFlowRateCalculator(systemConfiguratorRepository.Object);

            // act
            var result = calculator.Calculate(new SystemConfiguratorFlowRateData()
            {
                Crop = new Crop()
                {
                    CropFactor = 0.5m
                },
                Region = new Region()
                {
                    Eto = 6m
                },
                WeeklyIrrigationInterval = 7,
                RowSpacing = 3,
                MaxAllowedIrrigationTimePerDay = 10,
                PlotArea = 20
            });

            // assert
            Assert.AreEqual(67, result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Scenario_1_throws_exception_on_missing_data()
        {
            // arrange
            var systemConfiguratorRepository = new Mock<ISystemConfiguratorRepository>();
            systemConfiguratorRepository
                .Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>()))
                .Returns(new List<Product>
                {
                    new Dripperline()
                    {
                        FlowRate = 1.6m,
                        NumberOfLaterals = 1,
                        EmiterSpacing = 0.8m
                    }
                });

            var calculator = new ItalySystemConfiguratorFlowRateCalculator(systemConfiguratorRepository.Object);

            // act
            calculator.Calculate(new SystemConfiguratorFlowRateData());

            // assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Scenario_1_throws_exception_when_data_is_not_valid()
        {
            // arrange
            var systemConfiguratorRepository = new Mock<ISystemConfiguratorRepository>();
            systemConfiguratorRepository
                .Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>()))
                .Returns(new List<Product>
                {
                    new Dripperline()
                    {
                        FlowRate = 1.6m,
                        NumberOfLaterals = 1,
                        EmiterSpacing = 1000
                    }
                });

            var calculator = new ItalySystemConfiguratorFlowRateCalculator(systemConfiguratorRepository.Object);

            // act
            calculator.Calculate(new SystemConfiguratorFlowRateData()
            {
                Crop = new Crop()
                {
                    CropFactor = -5
                },
                Region = new Region()
                {
                    Eto = 6m
                },
                WeeklyIrrigationInterval = 1,
                RowSpacing = 3,
                MaxAllowedIrrigationTimePerDay = -20,
                PlotArea = 524
            });

            // assert
            // throws exception
        }

        /*
            scenario 2

            input:
                Location: BARLETTA
                Crop: Noce
                Plot area size: 6 ha 
                Row spacing: 7 m
                Max. allowed irrigation time per day: 2 hrs
                Weekly irrigation interval: 4 day
                Filtration type (Automatic or manual): AUTOMATIC
                Water source (well or surface water): SURFACE WATER
            output: 67
         */

        [Ignore] // test failed, waiting on update logic from Netafim
        [TestMethod]
        public void Scenario_2_return_result_for_valid_data()
        {
            // arrange
            var systemConfiguratorRepository = new Mock<ISystemConfiguratorRepository>();
            systemConfiguratorRepository
                .Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CultureInfo>()))
                .Returns(new List<Product>
                {
                    new Dripperline()
                    {
                        FlowRate = 1.6m,
                        NumberOfLaterals = 2,
                        EmiterSpacing = 0.7m
                    }
                });

            var calculator = new ItalySystemConfiguratorFlowRateCalculator(systemConfiguratorRepository.Object);

            // act
            var result = calculator.Calculate(new SystemConfiguratorFlowRateData()
            {
                Crop = new Crop()
                {
                    CropFactor = 1m
                },
                Region = new Region()
                {
                    Eto = 9m
                },
                WeeklyIrrigationInterval = 4,
                RowSpacing = 7,
                MaxAllowedIrrigationTimePerDay = 2,
                PlotArea = 6
            });

            // assert
            Assert.AreEqual(67, result);
        }
    }
}