module Netafim.Web {

    interface IProductCategorySearch {
        blockId: number,
        searchUrl: string,
    }
    
    export class ProductCategoryListing {

        params: IProductCategorySearch;
        constructor(jqueryElement: any, parameter: IProductCategorySearch) {
            this.params = parameter;
            let self = this;
            //Crop over-view .loadmore-btn
            $('.dropdown-category', jqueryElement).on('change', function () {
                self.searchProductCategory(jqueryElement);
            });

            this.searchProductCategory(jqueryElement);
        }

        private searchProductCategory(jqueryElement) {
            let data = {
                ProductFamilyId: $('.dropdown-category', jqueryElement).val(),
                BlockId: this.params.blockId
            };

            $.ajax({
                url: '/' + this.params.searchUrl,
                data: data,
                dataType: 'html',
                method: 'POST',
                cache: false,
                success: function (res) {
                    $(".data-result", jqueryElement).html(res);
                },
                error: function (jqXHR) {

                }
            });

        }
    }
}