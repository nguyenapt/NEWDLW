using System;
using EPiServer;
using EPiServer.Find;
using Netafim.WebPlatform.Web.Core.Templates;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Find;
using Netafim.WebPlatform.Web.Infrastructure.Epi.Shell.ViewModels;
using EPiServer.Core;
using System.Linq;
using Netafim.WebPlatform.Web.Features.ProductFamily;

namespace Netafim.WebPlatform.Web.Features.ProductCategory
{
    public class ProductCategoryListingQueryComposer : QueryComposerBase<ProductCategoryListingBlock, ProductCategoryListingQueryViewModel>
    {
        public ProductCategoryListingQueryComposer(IContentLoader contentLoader) : base(contentLoader)
        {
        }

        public override FilterExpression<ICanBeSearched> Compose(QueryViewModel query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var categoriesQuery = query as ProductCategoryListingQueryViewModel;

            if (categoriesQuery == null)
                throw new ArgumentException($"The expected type of query parameter is {nameof(ProductCategoryListingQueryViewModel)}");

            if (categoriesQuery.ProductFamilyId > 0)
            {
                var productFamily = this.ContentLoader.Get<ProductFamilyPage>(new ContentReference(categoriesQuery.ProductFamilyId));

                if (productFamily == null)
                    throw new ArgumentException($"Can not find any product family with id {categoriesQuery.ProductFamilyId}");

                var productCategories = productFamily.ProductCategories?.FilteredItems?.Select(m => m.GetContent())?.OfType<ProductCategoryPage>();
                
                var productCategoryIds = productCategories != null ? productCategories.Select(c => c.ContentLink.ID) : Enumerable.Empty<int>();

                return new FilterExpression<ICanBeSearched>(m => m.MatchType(typeof(ProductCategoryPage)) & ((ProductCategoryPage)m).ContentLink.ID.In(productCategoryIds));
            }

            return new FilterExpression<ICanBeSearched>(m => m.MatchType(typeof(ProductCategoryPage)));
        }       
    }
}