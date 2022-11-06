using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{
    public class DealerLocatorQueryViewModel : QueryViewModel
    {
        public double? Latitude { get; set; }

        public double? Longtitude { get; set; }

        public string SearchText { get; set; }

        public int RadiusSearch { get; set; }

        public bool HasGeoLocation() => this.Latitude.HasValue && this.Longtitude.HasValue;

        public bool HasSearchText() => !string.IsNullOrWhiteSpace(this.SearchText);
    }
}