using System.Collections.Generic;
using System.Globalization;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories.Impl.DefaultSystemConfiguratorRepository.Models;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Repositories
{
    public interface ISystemConfiguratorRepository
    {
        #region import

        /// <summary>
        /// Deletes all data for the given culture.
        /// </summary>
        /// <param name="culture"></param>
        void Delete(CultureInfo culture);

        void Store<T>(IEnumerable<T> entities) where T : SystemConfiguratorEntityBase; // SMELL correct to use this base type here? is EF

        #endregion

        #region controller

        IEnumerable<Crop> GetAllCrops(CultureInfo culture);

        IEnumerable<Region> GetAllRegions(CultureInfo culture);

        IEnumerable<FiltrationType> GetAllFiltrationTypes(CultureInfo culture);

        IEnumerable<WaterSource> GetAllWaterSources(CultureInfo culture);

        #endregion

        #region configurator

        IEnumerable<Contact> GetContacts(int cropId, int regionId, CultureInfo culture);

        IEnumerable<Dealer> GetDealers(int cropId, int regionId, CultureInfo culture);

        Crop GetCrop(int cropId);

        Region GetRegion(int regionId);

        FiltrationType GetFiltrationType(int filtrationTypeId);

        WaterSource GetWaterSource(int waterSourceId);

        IEnumerable<Product> GetProducts(int cropId, int regionId, CultureInfo culture);

        IEnumerable<Content> GetContent(int cropId, int regionId, CultureInfo culture);

        #endregion
    }
}