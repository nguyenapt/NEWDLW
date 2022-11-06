module Netafim.Web {

    interface IWeatherInput {
        blockId: number,
        floatingUrl: string,
        displayFloating: boolean,
    }

    export class Weather {
        
        constructor(jqueryElement: any, params: IWeatherInput) {
            function checkLessMoreBtn() {
                var less_text = $('.toggle-extra .loadmore-btn', jqueryElement).data('text-less');
                var more_text = $('.toggle-extra .loadmore-btn', jqueryElement).data('text-more');
                if ($('.toggle-extra .loadmore-btn', jqueryElement).hasClass('show-less')) {
                    $('.toggle-extra .loadmore-btn', jqueryElement).text(less_text);
                } else {
                    $('.toggle-extra .loadmore-btn', jqueryElement).text(more_text);
                }
            }
            $('.toggle-extra .loadmore-btn', jqueryElement).on('click', function () {
                $(this).toggleClass('show-less');
                $('.weather-item-container', jqueryElement).each(function () {
                    $(this).toggleClass('show-extra');
                });
                checkLessMoreBtn();
            });
            $('.weather-item-container', jqueryElement).on('click', function () {
                $('.weather-item-container', jqueryElement).toggleClass('show-extra');
                $('.toggle-extra .loadmore-btn', jqueryElement).toggleClass('show-less');
                checkLessMoreBtn();
            });


            if (params && params.displayFloating) {
                var currDate = new Date(Date.now());
                var url = '/' + params.floatingUrl + '?blockId=' + params.blockId + '&currentTime=' + currDate.toISOString();
                var queryString = window.location.href.slice(window.location.href.indexOf('?') + 1);
                url += '&' + queryString;

                $.ajax({
                    url: url,
                    data: {},
                    dataType: 'html',
                    method: 'GET',
                    cache: false,
                    success: function (res) {
                        $(".floating-weather-data", jqueryElement).html(res);
                    },
                    error: function (jqXHR) {

                    }
                });
            }
        }
    }
}