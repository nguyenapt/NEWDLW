using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class DefaultSystemConfiguratorService : ISystemConfiguratorService
    {
        private readonly ISystemConfiguratorRepository _systemConfiguratorRepository;
        private readonly ISystemConfiguratorProductsRetriever _systemConfiguratorProductsRetriever;

        public DefaultSystemConfiguratorService(
            ISystemConfiguratorRepository systemConfiguratorRepository, 
            ISystemConfiguratorProductsRetriever systemConfiguratorProductsRetriever)
        {
            _systemConfiguratorRepository = systemConfiguratorRepository;
            _systemConfiguratorProductsRetriever = systemConfiguratorProductsRetriever;
        }

        public SystemConfiguratorResult Process(SystemConfiguratorData data, CultureInfo culture)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            Validate(data);

            var products = GetProducts(data, culture);

            var contacts = GetContacts(data, culture);

            var dealers = GetDealers(data, culture);

            var solution = GetSolution(data, culture);

            var digitalFarming = GetDigitalFarming(data, culture);

            return new SystemConfiguratorResult
            {
                Solution = solution,
                DigitalFarming = digitalFarming,
                Products = products,
                Contacts = contacts,
                Dealers = dealers
            };
        }

        private DigitalFarming GetDigitalFarming(SystemConfiguratorData data, CultureInfo culture)
        {
            var digitalFarming = _systemConfiguratorRepository.GetContent(data.CropId, data.RegionId, culture)
                                    .FirstOrDefault(x => x.GetType() == typeof(DigitalFarming));

            if (digitalFarming == null) return null;

            return new DigitalFarming
            {
                ContentPage = digitalFarming.ContentPage
            };
        }

        private Solution GetSolution(SystemConfiguratorData data, CultureInfo culture)
        {
            var solution = _systemConfiguratorRepository.GetContent(data.CropId, data.RegionId, culture)
                .FirstOrDefault(x => x.GetType() == typeof(Solution));

            if (solution == null) return null;

            return new Solution
            {
                ContentPage = solution.ContentPage
            };
        }

        private IEnumerable<Dealer> GetDealers(SystemConfiguratorData data, CultureInfo culture)
        {
            return _systemConfiguratorProductsRetriever.GetDealers(data, culture);
        }

        private IEnumerable<Contact> GetContacts(SystemConfiguratorData data, CultureInfo culture)
        {
            return _systemConfiguratorRepository.GetContacts(data.CropId, data.RegionId, culture);
        }

        private IEnumerable<Product> GetProducts(SystemConfiguratorData data, CultureInfo culture)
        {
            return _systemConfiguratorProductsRetriever.GetProducts(data, culture);
        }

        internal void Validate(SystemConfiguratorData data)
        {
            if (data.RegionId <= 0)
                throw new ArgumentException("No region selected", nameof(data.RegionId));

            if (data.CropId <= 0)
                throw new ArgumentException("No crop selected", nameof(data.CropId));

            if (data.FiltrationTypeId <= 0)
                throw new ArgumentException("No filtration type selected", nameof(data.FiltrationTypeId));

            if (data.WaterSourceId <= 0)
                throw new ArgumentException("No water source selected", nameof(data.WaterSourceId));

            if (data.MaxAllowedIrrigationTimePerDay < 0 || data.MaxAllowedIrrigationTimePerDay > 24)
                throw new ArgumentException("Invalid input for maximum allowed irrigation time per day", nameof(data.MaxAllowedIrrigationTimePerDay));

            if (data.PlotArea < 0)
                throw new ArgumentException("Plot area must be a positive number", nameof(data.PlotArea));

            if (data.RowSpacing < 0)
                throw new ArgumentException("Row spacing must be a positive number", nameof(data.RowSpacing));

            if (data.WeeklyIrrigationInterval < 0)
                throw new ArgumentException("Weekly irrigation interval must be a positive number", nameof(data.WeeklyIrrigationInterval));
        }
    }
}