using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Shell
{
    [GroupDefinitions]
    public static class SharedTabs
    {
        [Display(Order = 100)]
        public const string SeoInformation = "Seo information";

        [Display(Order = 200)]
        public const string Preview = "Preview";
        
        [Display(Order = 300)]
        public const string CountryLanguageSelection = "Country Language Selection";

        [Display(Order = 400)]
        public const string Search = "Search";

        [Display(Order = 500)]
        public const string LayoutSettings = "Layout";

        [Display(Order = 600)]

        public const string Reference = "Reference";

        [Display(Order = 700)]

        public const string EmailInformation = "Email information";

        [Display(Order = 800)]
        public const string Event = "Event";

        [Display(Order = 900)]
        public const string Product = "Product";

        [Display(Order = 1000)]
        public const string EmailTemplate = "Email settings";

        [Display(Order = 1010)]
        public const string SuccessStoryContentOverview = "Content overview";

        [Display(Order = 1020)]
        public const string FloatingSettings = "Floating settings";

        [Display(Order = 1030)]
        public const string SystemConfiguratorSettings = "System configurator";
    }
}