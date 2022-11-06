using System.ComponentModel.DataAnnotations;
using EPiServer.DataAnnotations;
using EPiServer.Core;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Templates;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Features.OfficeLocator.Services;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    [ContentType(DisplayName = "Office location", GUID = "400bca16-1f49-48a3-b0e1-c1a7fe17a23a", Description = "The webmaster can input the location of the office")]
    [AvailableContentTypes(ExcludeOn = new[]
    {
        typeof(GenericContainerPage)
    })]
    public class OfficeLocatorPage : PageData
    {
        [CultureSpecific]
        [Display(Name = "Office's Name", Order = 10)]
        public virtual string OfficeName { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Address", Order = 20)]
        public virtual string Address { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Telephone", Order = 30)]
        public virtual string Phone { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Fax", Order = 40)]
        public virtual string Fax { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Email", Order = 50)]
        public virtual string Email { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Website", Order = 60)]
        public virtual string Website { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Direction", Order = 70)]
        public virtual string Direction { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Longtitude", Order = 80)]
        public virtual double Longtitude { get; set; }

        [CultureSpecific]
        [Display(Name = "Office's Latitude", Order = 90)]
        public virtual double Latitude { get; set; }

        [AutoSuggestSelection(typeof(OfficeCountrySelectionQuery))]
        [Display(Name = "Office's country", Order = 100)]
        public virtual string Country { get; set; }

        [ScaffoldColumn(false)]
        public virtual GeoLocation LocationForSearch
        {
            get
            {
                return new GeoLocation(Latitude, Longtitude);
            }
        }
    }
}