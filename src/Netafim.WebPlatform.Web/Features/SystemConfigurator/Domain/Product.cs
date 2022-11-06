using EPiServer.Core;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.Domain
{
    public abstract class Product
    {
        public string CatalogNumber { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Content page that represents the product
        /// </summary>
        public ContentReference ContentPage { get; set; }
    }
}