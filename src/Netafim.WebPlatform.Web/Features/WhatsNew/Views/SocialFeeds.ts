module Netafim.Web {

    interface ISocialFeedsFetch {
        blockId: number,
        fetchUrl: string,
    }
    export class SocialFeeds {
        params: ISocialFeedsFetch;
        constructor(jqueryElement: any, parameter: ISocialFeedsFetch) {
            this.params = parameter;
            let self = this;

            this.fetchFeeds(jqueryElement);           
        }
        private fetchFeeds(jqueryElement) {
            let data = {
                BlockId: this.params.blockId
            }
            $.ajax({
                url: '/' + this.params.fetchUrl,
                data: data,
                dataType: 'html',
                method: 'GET',
                cache: false,
                success: function (res) {
                    $(".data-result", jqueryElement).html(res);

                    $('.article-item', jqueryElement).on('click', function () {
                        var url = $(this).data('href');
                        if (url != undefined && url != null) {
                            window.open(url, '_blank');
                            return false;
                        }
                    });            
                },
                error: function (jqXHR) {

                }
            });
        }
    }
}