module Netafim.Web {
    export class HotspotSystem {
        constructor(jqueryElement: any) {
            var css_string = '<style type="text/css">@media (min-width: 1024px){';

            $('.hotspot', jqueryElement).each(function () {
                var img_width = $(this).parent().find('.clickable-image-bg').data('width');
                var img_height = $(this).parent().find('.clickable-image-bg').data('height');
                if ($(this).data('x') < (img_width - 140)) {
                    css_string += '#' + $(this).attr('id') + '{top:calc(' + $(this).data('y') / img_height * 100 + '% - 17px);left:calc(' + $(this).data('x') / img_width * 100 + '% - 17px);}';
                } else {
                    css_string += '#' + $(this).attr('id') + '{top:calc(' + $(this).data('y') / img_height * 100 + '% - 17px);left:calc(100% - 140px);}';
                }
            });
            
            $('.hotspot-link', jqueryElement).each(function () {
                var img_width = $(this).parent().find('.clickable-image-bg').data('width');
                var img_height = $(this).parent().find('.clickable-image-bg').data('height');
                css_string += '#' + $(this).attr('id') + '{top:calc(' + $(this).data('y') / img_height * 100 + '% - 28px);left:calc(' + $(this).data('x') / img_width * 100 + '% - 28px);}';
            });

            css_string += "}</style>";
            $(css_string).appendTo("head");

            //We need to disable "overflow" of the component container
            $('.clickable-image', jqueryElement).parents('.component').css('overflow', 'visible');

            //Click to toggle hotspot-desc
            $('.hotspot', jqueryElement).on('click', function (e) {
                if ($(e.target).parent().addBack().is('.hotspot-desc')) {
                    e.stopPropagation();
                } else {
                    if ($(this).hasClass('hotspot-selected')) {
                        e.stopPropagation();
                    } else {
                        $('.hotspot-selected').removeClass('hotspot-selected');
                        $(this).addClass('hotspot-selected');
                        var max_width = $(this).closest('.clickable-image').width() - 140;
                        var max_height = parseInt($(this).parent().find('.clickable-image-bg').data('height')) / 2;
                        //Check position to avoid hiding .hotspot-desc
                        if ($(this).offset().left <= 180) {
                            $(this).find('.hotspot-desc').css('left', 0);
                        }
                        if ($(this).data('x') >= max_width) {
                            $(this).find('.hotspot-desc').css('right', 0);
                            $(this).find('.hotspot-desc').css('left', 'auto');
                        }
                        //Setup arrow-up or arrow-down
                        if ($(this).data('y') <= max_height) {
                            $(this).find('.hotspot-desc').addClass('arrow-up');
                        } else {
                            $(this).find('.hotspot-desc').addClass('arrow-down');
                        }
                    }
                }
            });
            $('.hotspot .hotspot-desc .btn-close').on('click', function () {
                $(this).parents('.hotspot').removeClass('hotspot-selected');
            });
        }
    }
}