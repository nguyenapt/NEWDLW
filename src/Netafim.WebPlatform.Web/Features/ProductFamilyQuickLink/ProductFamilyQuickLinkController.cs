using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Netafim.WebPlatform.Web.Core.Extensions;
using Netafim.WebPlatform.Web.Features.ProductCategory;
using Netafim.WebPlatform.Web.Features.ProductFamily;

namespace Netafim.WebPlatform.Web.Features.ProductFamilyQuickLink
{
    public class ProductFamilyQuickLinkController : BlockController<ProductFamilyQuickLinkBlock>
    {
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly IProductFamilyRepository _productFamilyRepo;
        private readonly UrlResolver _urlResolver;

        public ProductFamilyQuickLinkController(IPageRouteHelper pageRouteHelper,
            IProductFamilyRepository productFamilyRepo,
            UrlResolver urlResolver)
        {
            _pageRouteHelper = pageRouteHelper;
            _productFamilyRepo = productFamilyRepo;
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(ProductFamilyQuickLinkBlock currentBlock)
        {
            var productCategory = _pageRouteHelper.Page as ProductCategoryPage;
            if (productCategory == null)
                return PartialView(currentBlock.GetDefaultFullViewName(), new ProductFamilyQuickLinkModel());

            var model = new ProductFamilyQuickLinkModel
            {
                Title = currentBlock.Title,
                Items = GetProductFamilies(productCategory)
            };

            return PartialView(currentBlock.GetDefaultFullViewName(), model);
        }

        private IEnumerable<ProductFamilyQuickLinkItem> GetProductFamilies(ProductCategoryPage productCategory)
        {
            var results = _productFamilyRepo.GetAllProductFamilyByCategoryPage(productCategory);

            return results.Select(t => new ProductFamilyQuickLinkItem
            {
                Text = string.IsNullOrEmpty(t.Title) ? t.PageName : t.Title,
                Link = _urlResolver.GetUrl(t.ContentLink)
            });
        }
    }
}