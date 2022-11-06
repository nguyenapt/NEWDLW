using System.Collections.Generic;
using System.Globalization;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services
{
    public interface ISystemConfiguratorProductsRetriever
    {
        IEnumerable<Product> GetProducts(SystemConfiguratorData data, CultureInfo culture);

        IEnumerable<Dealer> GetDealers(SystemConfiguratorData data, CultureInfo culture);
    }
}