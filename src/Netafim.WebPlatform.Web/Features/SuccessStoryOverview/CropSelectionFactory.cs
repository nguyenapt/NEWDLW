using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Dlw.EpiBase.Content.Cms.Search;
using EPiServer.Find;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Netafim.WebPlatform.Web.Features.CropsOverview;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    public class CropSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var pageService = ServiceLocator.Current.GetInstance<IPageService>();
            var findSettings = ServiceLocator.Current.GetInstance<IFindSettings>();
            var cropPages = pageService.GetPages<CropsPage>(findSettings.MaxItemsPerRequest, m => m.MatchType(typeof(CropsPage)));
            if (cropPages.IsNullOrEmpty())
                return Enumerable.Empty<ISelectItem>();

            return cropPages.Select(crop => new SelectItem
            {
                Value = crop.ContentLink.ID,
                Text = string.IsNullOrEmpty(crop.Title) ? crop.PageName : crop.Title
            });
        }
    }
}