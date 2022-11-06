using System.Collections.Generic;
using Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Services.Impl
{
    public class SystemConfiguratorResult
    {
        public Solution Solution { get; set; }

        public DigitalFarming DigitalFarming { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<Dealer> Dealers { get; set; }
    }
}