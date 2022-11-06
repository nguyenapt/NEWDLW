using System;
using System.Linq;
using Netafim.WebPlatform.Web.Core.Exceptions;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class ItalySystemConfiguratorFlowRateCalculator : ISystemConfiguratorFlowRateCalculator
    {
        private readonly ISystemConfiguratorRepository _systemConfiguratorRepository;

        public ItalySystemConfiguratorFlowRateCalculator(ISystemConfiguratorRepository systemConfiguratorRepository)
        {
            _systemConfiguratorRepository = systemConfiguratorRepository;
        }

        public decimal Calculate(SystemConfiguratorFlowRateData data)
        {
            // italy has for every crop a fixed dripperline
            var dripperLine = _systemConfiguratorRepository.GetProducts(data.Crop.Id, data.Region.Id, data.Culture).OfType<Domain.Dripperline>().SingleOrDefault();

            if (dripperLine == null) throw new BusinessException(BusinessExceptionCode.SystemConfigurator.NoDripperLineFoundForCrop, $"No dripperline found for crop '{data.Crop.Id}', region '{data.Region.Id}' and culture '{data.Culture}'.");

            try
            {
                var maxWaterRequirementPerShift = CalculateMaxWaterRequirementPerShift(data.Crop.CropFactor, data.Region.Eto, data.WeeklyIrrigationInterval);
                var irrigationRate = CalculateIrrigationRate(dripperLine.FlowRate, dripperLine.NumberOfLaterals, dripperLine.EmiterSpacing, data.RowSpacing);
                var shiftDuration = CalculateShiftDuration(maxWaterRequirementPerShift, irrigationRate);
                var numberOfShifts = CalculateNumberOfShifts(data.MaxAllowedIrrigationTimePerDay, shiftDuration);
                return CalculateShiftFlowRate(irrigationRate, data.PlotArea, numberOfShifts);

            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BusinessException(BusinessExceptionCode.SystemConfigurator.WrongInputValues, "Wrong input values, please review.", e);
            }
        }

        private static decimal CalculateMaxWaterRequirementPerShift(decimal kc, decimal eto, int wii)
        {
            if (kc < 0 || eto < 0 || wii < 0) throw new ArgumentException();

            return kc * eto * 7 / wii;
        }

        private static decimal CalculateIrrigationRate(decimal k0, decimal lpr, decimal sp, decimal rsp)
        {
            if (k0 < 0 || lpr < 0 || sp < 0 || rsp < 0) throw new ArgumentException();

            return Math.Round(k0 * lpr / (sp * rsp), 2);
        }

        private static decimal CalculateShiftDuration(decimal wrq, decimal irr)
        {
            if (wrq < 0 || irr < 0) throw new ArgumentException();

            return wrq / irr;
        }

        private static int CalculateNumberOfShifts(decimal hqd, decimal shd)
        {
            if(hqd < 0 || shd < 0) throw new ArgumentException();

            var numberOfShifts = Decimal.ToInt32(hqd / shd);

            if (numberOfShifts == 0)
            {
                throw new BusinessException(BusinessExceptionCode.SystemConfigurator.InvalidNumberOfShifts, "Calculated number of shifts is invalid.");
            }

            return numberOfShifts;
        }

        private static decimal CalculateShiftFlowRate(decimal irr, int area, int sh)
        {
            if (irr < 0 || area < 0 || sh < 0) throw new ArgumentException();
            
            return irr * area * 10 / sh;
        }
    }
}