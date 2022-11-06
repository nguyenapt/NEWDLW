module Netafim.Web {

    export class GenericListing {
        constructor(jqueryElement: any) {
            //Crop over-view .loadmore-btn
            $('.loadmore-crop-overview .loadmore-btn', jqueryElement).on('click', function () {

                var url = $(jqueryElement).attr('url-action');
                var data = JSON.parse($('#query', jqueryElement).val());
                $.ajax({
                    url: url,
                    data: data,
                    dataType: 'html',
                    method: 'POST',
                    cache: false,
                    success: function (res) {
                        var newQuery = $(res).find('#query');
                        if (newQuery) {
                            $('#query', jqueryElement).val(newQuery.val());
                        }

                        if ($(res).find('#endOfResult').length) { // Last page --> remove the show more view
                            $('#cropOverViewShowMore', jqueryElement).remove();
                        }

                        var html = $(res).find('.crop-overview-list').html();
                        $(html).appendTo('.crop-overview-list');

                        fixFlexLayout();
                    },
                    error: function (jqXHR) {

                    }
                })
            });

            //Switch view between grid and list modes
            $('.view-mode .grid-mode').on('click', function () {
                $('.selected-view-mode').removeClass('selected-view-mode');
                $(this).addClass('selected-view-mode');
                $('.crop-overview-list').attr('class', 'crop-overview-list grid-view');
                fixFlexLayout();
            });
            $('.view-mode .list-mode').on('click', function () {
                $('.selected-view-mode').removeClass('selected-view-mode');
                $(this).addClass('selected-view-mode');
                $('.crop-overview-list').attr('class', 'crop-overview-list list-view');
            });

            function fixFlexLayout() {
                var emptyItem;
                $('.crop-overview-item.is-empty').remove();
                $('.crop-overview-list.grid-view').each(function () {
                    emptyItem = [];
                    for (var i = 0; i < $(this).find('.crop-overview-item').length; i++) {
                        emptyItem.push($('<li class="crop-overview-item is-empty"></li>'));
                    }
                    $(this).append(emptyItem);
                });
            }
        }
    }
}