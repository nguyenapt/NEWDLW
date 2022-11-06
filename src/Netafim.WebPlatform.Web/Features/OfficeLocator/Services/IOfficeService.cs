using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator.Services
{
    public interface IOfficeService
    {
        IEnumerable<OfficeLocatorPage> Search(string countryCode, double? longtitude, double? latitude);
        IEnumerable<OfficeLocatorPage> Search();
    }
}
