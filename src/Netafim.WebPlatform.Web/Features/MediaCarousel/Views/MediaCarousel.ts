module Netafim.Web {
    export class MediaCarousel {
        constructor(jqueryElement: any) {
            //Media carousel
            //Only generate carousel controllers when there are more than ONE .media-carousel-item
            function generateCarouselNavigation(target_wrapper) {
                var newHtml = '';
                $(target_wrapper).find('.media-carousel-item').each(function () {
                    newHtml += $(this).clone().wrap('<div/>').parent().html();
                });
                newHtml += $(target_wrapper).find('.carousel-controller').clone().wrap('<div/>').parent().html();
                newHtml += $(target_wrapper).find('.carousel-navigation').clone().wrap('<div/>').parent().html();
                $(target_wrapper).html(newHtml);

                var number_of_dots = $(target_wrapper).find('.media-carousel-item').length;
                $(target_wrapper).find('.media-carousel-item').first().addClass('carousel-active');
                //If the first item has an auto-play video
                var first_active_item = $(target_wrapper).find('.media-carousel-item.carousel-active');
                if (!$(target_wrapper).find('.carousel-controller').find('.btn-carousel-prev').is(':visible')) {
                    if (first_active_item.attr("data-video-auto-play") === "true") {
                        first_active_item.find('video').show().trigger("play");
                        first_active_item.find('.btn-play').hide();
                        if (first_active_item.attr("data-show-text-onplay") === "true") {
                            first_active_item.find('.media-carousel-content').show();
                            first_active_item.find('.media-carousel-title').show();
                            first_active_item.find('.media-description').show();
                        } else {
                            first_active_item.find('.media-carousel-content').hide();
                            first_active_item.find('.media-carousel-title').hide();
                            first_active_item.find('.media-description').hide();
                        }
                    }
                }
                if (number_of_dots > 1) {
                    $(target_wrapper).find('.sliding-dots').css('height', number_of_dots * 100);
                    $(target_wrapper).find('.sliding-item .carousel-extra').html($(target_wrapper).find('.media-carousel-item').first().find('.carousel-extra-content').html());

                    $(target_wrapper).find('.carousel-dot').on('click', function () {
                        $(target_wrapper).find('.sliding-item .carousel-extra').html('');
                        var pos_top = $(this).position().top;
                        var current_index = $(this).index() - 1;
                        var selected_carousel_item = $(target_wrapper).find('.media-carousel-item:eq(' + current_index + ')').find('.carousel-extra-content').html();
                        $(target_wrapper).find('.sliding-item').css('top', pos_top);
                        $(target_wrapper).find('.sliding-item .carousel-extra').html(selected_carousel_item);

                        $(target_wrapper).find('.media-carousel-item.carousel-active').removeClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item:eq(' + current_index + ')').addClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item video').trigger('pause');
                        //If autoplay attribute is set
                        if ($(target_wrapper).find('.media-carousel-item.carousel-active').attr("data-video-auto-play") === "true") {
                            $(target_wrapper).find('.media-carousel-item.carousel-active video').show().trigger("play");
                            if ($(target_wrapper).find('.media-carousel-item.carousel-active').attr("data-show-text-onplay") === "true") {
                                $(target_wrapper).find('.media-carousel-item.carousel-active .btn-play').hide();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-title').show();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-content').show();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-description').show();
                            } else {
                                $(target_wrapper).find('.media-carousel-item.carousel-active .btn-play').hide();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-title').hide();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-content').hide();
                                $(target_wrapper).find('.media-carousel-item.carousel-active .media-description').hide();
                            }
                        } else {
                            $(target_wrapper).find('.media-carousel-item.carousel-active video').hide().trigger("pause");
                            $(target_wrapper).find('.media-carousel-item.carousel-active .btn-play').show();
                            $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-title').show();
                            $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-content').show();
                        }
                    });

                    $(target_wrapper).find('.carousel-controller').find('.btn-carousel-prev').on('click', function () {
                        var current_item_index = $(target_wrapper).find('.media-carousel-item.carousel-active').index();
                        if (current_item_index === 0) {
                            current_item_index = number_of_dots - 1;
                        } else {
                            current_item_index = current_item_index - 1;
                        }
                        $(target_wrapper).find('.media-carousel-item.carousel-active').removeClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item:eq(' + current_item_index + ')').addClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item video').trigger('pause');
                        $(target_wrapper).find('.media-carousel-item.carousel-active video').hide();
                        $(target_wrapper).find('.media-carousel-item.carousel-active img.banner-img').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .media-container .banner-bg').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .fluid-video img').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .video-control-play').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-content').show();
                    });

                    $(target_wrapper).find('.carousel-controller').find('.btn-carousel-next').on('click', function () {
                        var current_item_index = $(target_wrapper).find('.media-carousel-item.carousel-active').index();
                        if (current_item_index === (number_of_dots - 1)) {
                            current_item_index = 0;
                        } else {
                            current_item_index = current_item_index + 1;
                        }
                        $(target_wrapper).find('.media-carousel-item.carousel-active').removeClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item:eq(' + current_item_index + ')').addClass('carousel-active');
                        $(target_wrapper).find('.media-carousel-item video').trigger('pause');
                        $(target_wrapper).find('.media-carousel-item.carousel-active video').hide();
                        $(target_wrapper).find('.media-carousel-item.carousel-active img.banner-img').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .media-container .banner-bg').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .fluid-video img').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .video-control-play').show();
                        $(target_wrapper).find('.media-carousel-item.carousel-active .media-carousel-content').show();
                    });
                }
                else {
                    $(target_wrapper).find('.carousel-navigation').hide();
                    $(target_wrapper).find('.carousel-controller').hide();
                }

                //Update video scale on IE
                var ua = window.navigator.userAgent;
                var trident = ua.indexOf('Trident/');
                var edge = ua.indexOf('Edge/');
                var _windowRatio = $(window).width() / $(window).height();
                if (trident > 0 || edge > 0) {
                    // IE 11
                    $(target_wrapper).find('.media-carousel-item').each(function () {
                        var _video = $(this).find('video');
                        if ($(target_wrapper).hasClass('hero-banner')) {
                            _video.on('loadedmetadata', function () {
                                var _videoRatio = this.videoWidth / this.videoHeight;
                                if (_videoRatio >= _windowRatio) {
                                    $(this).css('height', $(window).height());
                                } else {
                                    $(this).css('width', $(window).width());
                                }
                            });
                        } else {
                            _video.on('loadedmetadata', function () {
                                var _videoRatio = this.videoWidth / this.videoHeight;
                                var _videoContainerWidth = $(this).parent().width();
                                var _videoContainerHeight = $(this).parent().height();
                                var _carouselRatio = _videoContainerWidth / _videoContainerHeight;
                                $(this).css('position', 'absolute');
                                $(this).css('z-index', 0);
                                $(this).css('height', 'auto');
                                if (_videoRatio >= _carouselRatio) {
                                    $(this).css('height', _videoContainerHeight);
                                } else {
                                    $(this).css('width', _videoContainerWidth);
                                }
                            });
                        }

                    });
                }
            }

            generateCarouselNavigation($('.media-carousel-wrapper', jqueryElement));

            $('.btn-play', jqueryElement).on('click', function () {
                $(this).hide();
                if ($(this).parents('.media-carousel-item.carousel-active').attr("data-show-text-onplay") === "true") {
                    $(this).parents('.media-carousel-item.carousel-active').find('.video-container img').hide();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').show();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').trigger('play');
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-title').show();
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-content').show();
                } else {
                    $(this).parents('.media-carousel-item.carousel-active').find('.video-container img').hide();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').show();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').trigger('play');
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-title').hide();
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-content').hide();
                }
            });
            $('.fluid-img', jqueryElement).each(function () {
                $(this).css('background-image', 'url(' + $(this).find('img').attr('src') + ')');
            });
            $('.fluid-video', jqueryElement).each(function () {
                $(this).css('background-image', 'url(' + $(this).find('video').attr('poster') + ')');
            });
            $('.video-control-play', jqueryElement).on('click', function () {
                $(this).hide();
                //For richtext component
                if ($(this).parents('.media-carousel-item').length) {
                    $(this).parents('.media-carousel-item.carousel-active').find('.video-container img').hide();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').show();
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-content').show();
                    $(this).parents('.media-carousel-item.carousel-active').find('video').trigger('play');
                    $(this).parents('.media-carousel-item.carousel-active').find('.media-carousel-title').hide();
                } else {
                    //For media-carousel-item
                    $(this).parents('.fluid-video').find('img').hide();
                    $(this).parents('.fluid-video').find('video').css('position', 'relative').show();
                    $(this).parents('.fluid-video').find('video').trigger('play');
                }
            });
        }
    }
}