using EPiServer.Web.Routing;
using System.Linq;
using EPiServer.Core.Internal;
using Netafim.WebPlatform.Web.Features.OfficeLocator.Services;
using System;
using Netafim.WebPlatform.Web.Core.Extensions;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Netafim.WebPlatform.Web.Core.Services;

namespace Netafim.WebPlatform.Web.Features.OfficeLocator
{
    public class OfficeLocatorController : BlockController<OfficeLocatorBlock>
    {
        protected readonly IOfficeService OfficeService;
        protected readonly IObjectSerialization ObjectSerialization;
        protected readonly IOfficeSettings OfficeSettings;
        protected readonly UrlResolver UrlResolver;

        public OfficeLocatorController(IOfficeService officeService,
            IObjectSerialization objectSerialization, UrlResolver urlResolver,
            IOfficeSettings officeSettings)
        {
            this.OfficeService = officeService;
            this.ObjectSerialization = objectSerialization;
            this.UrlResolver = urlResolver;
            this.OfficeSettings = officeSettings;
        }


        public override ActionResult Index(OfficeLocatorBlock currentContent)
        {
            return PartialView(currentContent.GetDefaultViewName(), currentContent);
        }

        [HttpPost]
        public ActionResult SearchOffices(SearchCriteriaViewModel query)
        {
            var hasCriteria = query.Latitude.HasValue && query.Longtitude.HasValue;
            var offices = hasCriteria ? this.OfficeService.Search(query.Country, query.Longtitude.Value, query.Latitude.Value) : this.OfficeService.Search();

            var officeViewModels = offices.Select(MapToViewModel);
            
            return Json(ObjectSerialization.ToJson(officeViewModels));
        }
        
        private OfficeDetailViewModel MapToViewModel(OfficeLocatorPage office)
        {
            return new OfficeDetailViewModel()
            {
                Address = office.Address,
                Direction = office.Direction,
                Email = office.Email,
                Fax = office.Fax,
                Longtitude = office.Longtitude,
                Latitude = office.Latitude,
                OfficeName = office.Name,
                Phone = office.Phone,
                Website = office.Website
            };
        }
    }
}