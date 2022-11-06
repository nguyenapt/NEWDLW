module Netafim.WebPlatform.Web.Features {

    const ID_QUICKSEARCH_BUTTON: string = '#js-quickSearchButton';

    interface IKeywordSearch {
        url: string;
    }

    export class KeywordSearch {

        parameters: IKeywordSearch;

        constructor(jqueryElement: any, params: IKeywordSearch) {

            this.parameters = params;

            jqueryElement.on("click", ID_QUICKSEARCH_BUTTON, e => {
                e.preventDefault();
                var url = this.parameters.url + "?SearchText=" + jqueryElement.find("#js-searchTextField").val();
                window.location.href = url;
            });
        }
    }
}