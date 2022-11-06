using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.UnitTest.Web.Features.SystemConfigurator.Services
{
    [TestClass]
    public class DefaultSystemConfiguratorServiceTests
    {
        protected DefaultSystemConfiguratorService Service { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            this.Service = new DefaultSystemConfiguratorService(null, null);
        }

        private SystemConfiguratorData GetValidData()
        {
            var data = new SystemConfiguratorData()
            {
                RegionId = 8,
                CropId = 5,
                FiltrationTypeId = 3,
                WaterSourceId = 4
            };

            return data;
        }

        /// <summary>
        /// Simple test with valid input throwing no exception.
        /// </summary>
        [TestMethod]
        public void Validate_throws_no_exceptions_when_valid()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // no exception thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_region_is_invalid()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.RegionId = 0;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_crop_is_invalid()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.CropId = 0;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_max_irrigation_time_exceeds_maximum()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.MaxAllowedIrrigationTimePerDay = 25;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_max_irrigation_time_is_negative_number()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.MaxAllowedIrrigationTimePerDay = -1;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_row_spacing_is_negative_number()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.RowSpacing = -1;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_plot_area_is_negative_number()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.WeeklyIrrigationInterval = -1;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_throws_exception_when_weekly_irrigation_interval_is_negative_number()
        {
            // Arrange
            var configuratorData = this.GetValidData();

            configuratorData.WeeklyIrrigationInterval = -1;

            // Act
            this.Service.Validate(configuratorData);

            // Assert
            // throws exception
        }
    }
}
