module Netafim.Web {

    export class RichTextMediaBlock {
        constructor(jqueryElement: any) {
            $('.fluid-img', $(jqueryElement).parent()).each(function () {
                $(this).css('background-image', 'url(' + $(this).find('img').attr('src') + ')');
            });

            $('.fluid-video', $(jqueryElement).parent()).each(function () {
                $(this).css('background-image', 'url(' + $(this).find('img').attr('src') + ')');
            });

            
            $('.video-control-play', jqueryElement).on('click', function () {
                $(this).hide();
                $(this).parents('.fluid-video').find('img').hide();
                $(this).parents('.fluid-video').find('video').css('position', 'relative').show();
                $(this).parents('.fluid-video').find('video').trigger('play');
            });
        }
    }
}