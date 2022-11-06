module Netafim.WebPlatform.Web.Features {

    declare var dataLayer: any;
    const ID_SEARCH_BUTTON: string = '#js-searchButton';
    const CLASS_PAGE_LINK: string = '.js-pageLink';
    const CLASS_NEXT_PAGE: string = '.js-nextPage';
    const CLASS_PREVIOUS_PAGE: string = '.js-previousPage';
    const CLASS_CATEGORY_FACET_LIST_ITEM: string = '.js-categoryFacetList';
    const ID_CATEGORY_FACET_DROPDOWN: string = '#js-categoryFacetDropDown';

    interface ISearch {
        url: string;
        currentSearchText: string;
        isThisTheLastPage: boolean;
        numberOfSearchResults: number;
        currentPageNumber: number;
        currentCategory: number;
    }

    export class Search {

        parameters: ISearch;

        constructor(jqueryElement: any, params: ISearch) {

            this.parameters = params;

            $(document).ready(() => {
                if (this.parameters.currentSearchText) {
                    jqueryElement.find("#js-searchTextField").val(this.parameters.currentSearchText);
                }
            });

            jqueryElement.on("click", ID_SEARCH_BUTTON, e => {
                e.preventDefault();
                this.redirectToSearchResultPage(jqueryElement.find("#js-searchTextField").val(), null, null);
            });

            jqueryElement.on("click", CLASS_PREVIOUS_PAGE, e => {
                var page = this.parameters.currentPageNumber - 1;
                if (page < 1) return;
                e.preventDefault();
                this.redirectToSearchResultPage(this.parameters.currentSearchText, page, this.parameters.currentCategory);
            });

            jqueryElement.on("click", CLASS_NEXT_PAGE, e => {
                if (this.parameters.isThisTheLastPage) return;
                var page = this.parameters.currentPageNumber + 1;
                e.preventDefault();
                this.redirectToSearchResultPage(this.parameters.currentSearchText, page, this.parameters.currentCategory);
            });

            jqueryElement.on("click", CLASS_PAGE_LINK, e => {
                var page = $(e.currentTarget).attr("data-pageNumber");
                e.preventDefault();
                this.redirectToSearchResultPage(this.parameters.currentSearchText, parseInt(page), this.parameters.currentCategory);
            });

            $(jqueryElement).find(CLASS_CATEGORY_FACET_LIST_ITEM).on("click", (e) => {
                var categoryKey = $(e.currentTarget).attr("data-key");
                e.preventDefault();
                if (!categoryKey) categoryKey = null;
                this.redirectToSearchResultPage(this.parameters.currentSearchText, null, parseInt(categoryKey));
            });

            $(jqueryElement).find(ID_CATEGORY_FACET_DROPDOWN).on("change", (e) => {
                var categoryKey = $(e.currentTarget).find(":selected").val();
                e.preventDefault();
                this.redirectToSearchResultPage(this.parameters.currentSearchText, null, parseInt(categoryKey));
            });
        }

        private redirectToSearchResultPage(searchText: string, page: number, category: number) {
            var url = this.parameters.url + "?SearchText=" + searchText;
            if (page) {
                url = url + "&Page=" + page;
            }
            if (category) {
                url = url + "&Category=" + category;
            }
            window.location.href = url;
        }
    }
}