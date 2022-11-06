using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Netafim.WebPlatform.Web.Features.CropsOverview;

namespace Netafim.WebPlatform.Web.Features.SuccessStoryOverview
{
    public static class SuccessStoryOverviewExtensions
    {
        public static string RenderViewToString(this ControllerBase context, string viewName, object model)
        {
            context.ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var vResult = ViewEngines.Engines.FindPartialView(context.ControllerContext, viewName);
                var vContext = new ViewContext(context.ControllerContext, vResult.View, context.ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }

        public static string GetCropNameByCropId(this int cropId, IEnumerable<CropsPage> cropPages)
        {
            if(!cropPages.Any())
                return string.Empty;

            var crop = cropPages.FirstOrDefault(c => c.ContentLink.ID == cropId);
            if (crop == null)
                return string.Empty;

            return string.IsNullOrEmpty(crop.Title) ? crop.PageName : crop.Title;
        }

        public static Tuple<int, int> CalculateTakeAndSkipItem(int boostedItems, int boostedTotalMatching, int pageSize, int currentPage, bool hasHashData)
        {
            int takeRest;
            int skipRest = 0;

            // Boosted result still match enough
            if (boostedItems == pageSize)
            {
                return new Tuple<int, int>(0, 0);
            }

            var boostedTotalPage = CalculateTotalPage(boostedTotalMatching, pageSize) - 1;
            var deficitNumber = pageSize - (boostedTotalMatching % pageSize);
            
            if (boostedTotalPage == currentPage && deficitNumber > 0) // Boosted result return not match with pageSize
            {
                takeRest = deficitNumber;
            }
            else
            {
                // Calculate skip and take for query from the rest of items
                var currentNumberOfRest = (currentPage - boostedTotalPage) - 1;
                var takeWithDeficitNumber = TakeWithDeficitNumber(currentNumberOfRest, pageSize, deficitNumber);

                takeRest = hasHashData ? takeWithDeficitNumber : pageSize;
                skipRest = hasHashData ? 0 : takeWithDeficitNumber;
            }

            return new Tuple<int, int>(skipRest, takeRest);
        }

        private static int CalculateTotalPage(int totalMatching, int pageSize)
        {
            return (int)Math.Ceiling((double)totalMatching / pageSize);
        }

        private static int TakeWithDeficitNumber(int currentPage, int pageSize, int deficitNumber = 0)
        {
            return (currentPage*pageSize) + deficitNumber;
        }
    }
}