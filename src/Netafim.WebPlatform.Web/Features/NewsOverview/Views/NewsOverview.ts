module Netafim.Web {

    interface INewsOverviewSearch {
        blockId: number,
        searchUrl: string,
    }
    export class NewsOverview {

        params: INewsOverviewSearch;
        constructor(jqueryElement: any, parameter: INewsOverviewSearch) {
            this.params = parameter;
            let self = this;
            $(document).ready(function () {
                console.log('ready');
                var curActiveAccordion = $('.accordion-container .accordion-item.accordion-active', jqueryElement);
                if (curActiveAccordion.length > 0) {
                    self.searchNewsPages($(curActiveAccordion[0]));
                }
            });

            $('.accordion-container .accordion-item .accordion-item-title', jqueryElement).on('click', function (e) {
                var this_container = $(this).parent('.accordion-item');
                var this_contentItem = $(this).siblings('.result');
                if (!self.alreadyLoaded(this_contentItem)) {
                    // load news item.
                    self.searchNewsPages(this_container);
                }
            });

            //Accordion menu
            $('.accordion-container .accordion-item .accordion-item-title', jqueryElement).on('click', function (e) {
                e.preventDefault();
                var this_wrapper = $(this).parents('.accordion-container');
                var this_container = $(this).parent();
                if (this_container.hasClass('accordion-active')) {
                    this_wrapper.find('.accordion-item').removeClass('accordion-active');
                    this_container.addClass('accordion-active');
                }
            });

            $('.accordion-container .accordion-item .accordion-item-title', jqueryElement).on('click', function () {
                $(this).parent().toggleClass('accordion-active');
                $(this).parent().siblings().removeClass('accordion-active');
            });
        }

        private alreadyLoaded(this_contentItem): boolean {
            return $('.accordion-item-content', $(this_contentItem)).length > 0;
        }

        private searchNewsPages(container) {

            let data = {
                BlockId: this.params.blockId,
                Year: $(container).attr('data-value')
            };

            $.ajax({
                url: '/' + this.params.searchUrl,
                data: data,
                dataType: 'html',
                method: 'POST',
                cache: false,
                success: function (res) {
                    $(".data-result", container).html(res);
                },
                error: function (jqXHR) {

                }
            });
        }
    }
}