module Netafim.Web {
    export class Accordion {
        constructor(jqueryElement: any) {
            $('.accordion-container .accordion-item .accordion-item-title', jqueryElement).on('click', function () {
                if ($(this, jqueryElement).parent().hasClass('accordion-active')) {
                    $(this, jqueryElement).parent().removeClass('accordion-active');
                } else {
                    $(this, jqueryElement).parents('.accordion-container').find('.accordion-active').removeClass('accordion-active');
                    $(this, jqueryElement).parent().addClass('accordion-active');
                }
            });
        }
    }
}