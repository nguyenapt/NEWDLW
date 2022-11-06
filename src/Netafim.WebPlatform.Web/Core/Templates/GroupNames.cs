using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;

namespace Netafim.WebPlatform.Web.Core.Templates
{
    [GroupDefinitions]
    public static class GroupNames
    {
        [Display(Order = 100)]
        public const string Listers = "Listers";

        [Display(Order = 200)]
        public const string Infrastructure = "Infrastructure";

        [Display(Order = 300)]
        public const string Richtexts = "Rich Text";
        
        [Display(Order = 400)]
        public const string MediaCarousel = "Media carousel";

        [Display(Order = 450)]
        public const string Locator = "Locator";

        [Display(Order = 500)]
        public const string HeroBanerCarousel = "Hero banner carousel";

        [Display(Order = 600)]
        public const string Containers = "Containers";

        [Display(Order = 700)]
        public const string CropsOverview = "Crops overview";

        [Display(Order = 800)]
        public const string Overview = "Overview";

        [Display(Order = 900)]
        public const string Form = "Forms";

        [Display(Order = 1000)]
        public const string Accordion = "Accordion(FAQs, Product per application)";

        [Display(Order = 1100)]
        public const string Products = "Products";
        [Display(Order = 1000)]
        public const string JobInformation = "Job information";

        [Display(Order = 1100)]
        public const string Configurator = "Configurator";
    }
}