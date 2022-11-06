using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class ItalySystemConfiguratorProductsRetriever : ISystemConfiguratorProductsRetriever
    {
        private readonly ISystemConfiguratorRepository _systemConfiguratorRepository;
        private readonly ISystemConfiguratorFlowRateCalculator _configuratorFlowRateCalculator;

        public ItalySystemConfiguratorProductsRetriever(
            ISystemConfiguratorRepository systemConfiguratorRepository,
            ISystemConfiguratorFlowRateCalculator configuratorFlowRateCalculator)
        {
            _systemConfiguratorRepository = systemConfiguratorRepository;
            _configuratorFlowRateCalculator = configuratorFlowRateCalculator;
        }

        public IEnumerable<Product> GetProducts(SystemConfiguratorData data, CultureInfo culture)
        {
            var products = new List<Product>();
            if(data.RegionId < 0) throw new ArgumentException($"{nameof(data.RegionId)} has to be a positive integer.");
            if(data.CropId < 0) throw new ArgumentException($"{nameof(data.CropId)} has to be a positive integer.");
            if(data.FiltrationTypeId < 0) throw new ArgumentException($"{nameof(data.FiltrationTypeId)} has to be a positive integer.");
            if(data.WaterSourceId < 0) throw new ArgumentException($"{nameof(data.WaterSourceId)} has to be a positive integer.");
            if(culture == null) throw new ArgumentException($"{nameof(culture)} is required.");

            var crop = _systemConfiguratorRepository.GetCrop(data.CropId);
            var region = _systemConfiguratorRepository.GetRegion(data.RegionId);
            var filtrationType = _systemConfiguratorRepository.GetFiltrationType(data.FiltrationTypeId);
            var waterSource = _systemConfiguratorRepository.GetWaterSource(data.WaterSourceId);

            var flowrate = _configuratorFlowRateCalculator.Calculate(new SystemConfiguratorFlowRateData
            {
                Crop = crop,
                Region = region,
                PlotArea = data.PlotArea,
                MaxAllowedIrrigationTimePerDay = data.MaxAllowedIrrigationTimePerDay,
                RowSpacing = data.RowSpacing,
                WeeklyIrrigationInterval = data.WeeklyIrrigationInterval,
                FiltrationType = filtrationType,
                WaterSource = waterSource,
                Culture = culture
            });

            var allProducts = _systemConfiguratorRepository.GetProducts(data.CropId, data.RegionId, culture);
            products.AddRange(ReduceFiltrations(allProducts.ToList(), flowrate, filtrationType, waterSource));

            return products;
        }

        public IEnumerable<Dealer> GetDealers(SystemConfiguratorData data, CultureInfo culture)
        {
            return _systemConfiguratorRepository.GetDealers(data.CropId, data.RegionId, culture);
        }

        private IEnumerable<Product> ReduceFiltrations(IList<Product> products, decimal flowrate, FiltrationType filtrationType, WaterSource waterSource)
        {
            // extract relevant filtrations
            var filteredAndOrderedFiltrations = products.OfType<Filtration>()
                .Where(p => p.FiltrationType.Id == filtrationType.Id && p.WaterSource.Id == waterSource.Id)
                .OrderBy(p => p.FlowRate).ToList();

            if (filteredAndOrderedFiltrations.Any())
            {
                // flowrates are threated as ranges, duplicate last item to add top range
                var last = filteredAndOrderedFiltrations.Last();
                last.FlowRate = int.MaxValue;
                filteredAndOrderedFiltrations.Add(last);
            }

            var filtration = filteredAndOrderedFiltrations.FirstOrDefault(x => x.FlowRate >= flowrate);

            foreach (var product in products)
            {
                // ignore non matching filtrations
                if (product is Filtration) continue;

                yield return product;
            }

            // only one filtration is allowed in result
            if (filtration != null) yield return filtration;
        }
    }
}