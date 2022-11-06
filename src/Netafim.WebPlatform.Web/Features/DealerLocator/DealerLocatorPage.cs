using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.DataAnnotations;
using Netafim.WebPlatform.Web.Core.Templates;
using System.ComponentModel.DataAnnotations;

namespace Netafim.WebPlatform.Web.Features.DealerLocator
{

    [ContentType(DisplayName = "Dealer location", GUID = "00128ff5-7413-4888-a4fc-f5e408c20ed5", Description = "The webmaster can input the location of the dealer")]
    [AvailableContentTypes(ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class DealerLocatorPage : NoTemplatePageBase, ICanBeSearched
    {
        [CultureSpecific]
        [Display(Name = "Dealer's Logo (80x80)", Order = 10)]
        [AllowedTypes(typeof(ImageData))]
        [ImageMetadata(80, 80)]
        public virtual ContentReference Logo { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Name", Order = 20)]
        public virtual string DealerName { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Address", Order = 30)]
        public virtual string Address { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Telephone", Order = 40)]
        public virtual string Phone { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Dealer's Email", Order = 50)]
        public virtual string Email { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Website", Order = 60)]
        public virtual string Website { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Direction", Order = 70)]
        public virtual string Direction { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Longtitude", Order = 80)]
        public virtual double Longtitude { get; set; }

        [CultureSpecific]
        [Display(Name = "Dealer's Latitude", Order = 90)]
        public virtual double Latitude { get; set; }

        [ScaffoldColumn(false)]
        public virtual GeoLocation LocationForSearch
        {
            get
            {
                return new GeoLocation(Latitude, Longtitude);
            }
        }

        #region ICanBeSearched

        public string Title => this.DealerName;

        public string Summary => this.Address;

        public string Keywords => string.Empty;

        public virtual ContentReference Image => this.Logo; 
        
        #endregion
    }
}