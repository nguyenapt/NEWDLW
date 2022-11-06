module Netafim.Web {

    interface IProductFamilyMatrixSearch {
        blockId: number,
        searchUrl: string,
    }

    export class FamilyMatrix {


        params: IProductFamilyMatrixSearch;
        constructor(jqueryElement: any, parameter: IProductFamilyMatrixSearch) {
            this.params = parameter;
            let self = this;

            var productCategoryId = $("#product-category-id", jqueryElement).val();
            if (parseInt(productCategoryId) > 0) {
                //Crop over-view .loadmore-btn
                $('.FormSelection select', jqueryElement).on('change', function () {
                    self.searchProductFamily(jqueryElement);
                });

                this.searchProductFamily(jqueryElement);
            }

        }

        private searchProductFamily(jqueryElement) {
            let data = {
                Criteria: this.getAllCriteria(jqueryElement),
                BlockId: this.params.blockId,
                ProductCategoryId: $("#product-category-id", jqueryElement).val(),
                CriteriaTypeIds: this.getAllCriteriaTypeIds(jqueryElement)
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
        private getAllCriteria(jqueryElement): number[] {
            var criteriaIds: number[] = [];
            $('.FormSelection select', jqueryElement).each(function (idx, item) {
                criteriaIds[idx] = parseInt(($(item).val()).toString())
            })
            return criteriaIds;
        }
        getAllCriteriaTypeIds(jqueryElement): number[] {
            var criteriaTypeIds: number[] = [];
            $('.criteria-type', jqueryElement).each(function (idx, item) {
                criteriaTypeIds[idx] = parseInt(($(item).attr("data-value")).toString());
            })
            return criteriaTypeIds;
        }
    }
}