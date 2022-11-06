module Netafim.Web {
    interface ICarousel {
        defaultNumberOfItems: number,
        itemWidth: number,
        currentCarouselItem: number,
        moveCarousel: Function,
        updateArrow: Function
    }
    export class CarouselArrow {
        carouselObject:ICarousel;
        constructor(jqueryElement: any) {
            let self = this;
            this.carouselObject = {
                defaultNumberOfItems: 0,
                itemWidth: 0,
                currentCarouselItem: 0,
                moveCarousel: null,
                updateArrow: null
            };
            $('.carousel-wrapper', jqueryElement).each(function (index, carousel) {
                self.carouselObject.defaultNumberOfItems = 5;
                self.carouselObject.itemWidth = $(this).find('.link-box-item').outerWidth(true);
                self.carouselObject.currentCarouselItem = 0;

                self.carouselObject.moveCarousel = function (itemWidth, currentCarouselItem) {
                    var moveDistance = itemWidth * currentCarouselItem;
                    $(carousel).find('.link-box-item').css('transform', 'translateX(' + -moveDistance + 'px)');
                };
                self.carouselObject.updateArrow = function () {
                    if (self.carouselObject.currentCarouselItem === $(carousel).find('.link-box-item').length - self.carouselObject.defaultNumberOfItems) {
                        $(carousel).find('.nav-arrow-right').addClass('disabled-arrow');
                    } else {
                        $(carousel).find('.nav-arrow-right').removeClass('disabled-arrow');
                    }
                    if (self.carouselObject.currentCarouselItem === 0) {
                        $(carousel).find('.nav-arrow-left').addClass('disabled-arrow');
                    } else {
                        $(carousel).find('.nav-arrow-left').removeClass('disabled-arrow');
                    }
                };

                self.carouselObject.updateArrow();
                if ($(carousel).find('.link-box-item').length <= self.carouselObject.defaultNumberOfItems) {
                    $(carousel).find('.nav-arrow').addClass('hidden');
                    $(carousel).find('.items-list').addClass('link-box');
                } else {
                    $(carousel).find('.nav-arrow').removeClass('hidden');
                    $(carousel).find('.items-list').removeClass('link-box');
                }
                $(carousel).find('.nav-arrow-right').on('click', function () {
                    if (self.carouselObject.currentCarouselItem === $(carousel).find('.link-box-item').length - self.carouselObject.defaultNumberOfItems) {
                        return;
                    }
                    self.carouselObject.currentCarouselItem++;
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                    self.carouselObject.updateArrow();
                });
                $(carousel).find('.nav-arrow-left').on('click', function () {
                    if (self.carouselObject.currentCarouselItem === 0) {
                        return;
                    }
                    self.carouselObject.currentCarouselItem--;
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                    self.carouselObject.updateArrow();
                });
            });

            function updateHeight(carousel) {
                $(carousel).find('.item').css('height', '');
                var maxHeight = 0;
                $(carousel).find('.item').each(function () {
                    if ($(this).outerHeight() > maxHeight) {
                        maxHeight = $(this).outerHeight();
                    }
                });
                $(carousel).find('.item').css('height', maxHeight);
            }

            $('.three-items-carousel-wrapper', jqueryElement).each(function (index, carousel) {
                self.carouselObject.defaultNumberOfItems = 3;
                self.carouselObject.itemWidth = $(this).find('.item').outerWidth(true);
                self.carouselObject.currentCarouselItem = 0;
                self.carouselObject.moveCarousel = function (itemWidth, currentCarouselItem) {
                    var moveDistance = itemWidth * currentCarouselItem;
                    $(carousel).find('.item').css('transform', 'translateX(' + -moveDistance + 'px)');
                };
                self.carouselObject.updateArrow = function () {
                    if (self.carouselObject.currentCarouselItem === $(carousel).find('.item').length - self.carouselObject.defaultNumberOfItems) {
                        $(carousel).find('.nav-arrow-right').addClass('disabled-arrow');
                    } else {
                        $(carousel).find('.nav-arrow-right').removeClass('disabled-arrow');
                    }
                    if (self.carouselObject.currentCarouselItem === 0) {
                        $(carousel).find('.nav-arrow-left').addClass('disabled-arrow');
                    } else {
                        $(carousel).find('.nav-arrow-left').removeClass('disabled-arrow');
                    }
                };

                updateHeight(carousel);

                self.carouselObject.updateArrow();
                if ($(carousel).find('.item').length <= self.carouselObject.defaultNumberOfItems) {
                    $(carousel).find('.nav-arrow').addClass('hidden');
                } else {
                    $(carousel).find('.nav-arrow').removeClass('hidden');
                }
                $(carousel).find('.nav-arrow-right').on('click', function () {
                    if (self.carouselObject.currentCarouselItem === $(carousel).find('.item').length - self.carouselObject.defaultNumberOfItems) {
                        return;
                    }
                    self.carouselObject.currentCarouselItem++;
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                    self.carouselObject.updateArrow();
                });
                $(carousel).find('.nav-arrow-left').on('click', function () {
                    if (self.carouselObject.currentCarouselItem === 0) {
                        return;
                    }
                    self.carouselObject.currentCarouselItem--;
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                    self.carouselObject.updateArrow();
                });
            });

            function updateCarousel() {
                $('.carousel-wrapper', jqueryElement).each(function (index, carousel) {
                    self.carouselObject.itemWidth = $(carousel).find('.link-box-item').outerWidth(true);
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                });
                $('.three-items-carousel-wrapper', jqueryElement).each(function (index, carousel) {
                    self.carouselObject.itemWidth = $(carousel).find('.item').outerWidth(true);
                    self.carouselObject.moveCarousel(self.carouselObject.itemWidth, self.carouselObject.currentCarouselItem);
                    updateHeight(carousel);
                });
            }

            $(window).on('resize', function() {
                updateCarousel();
            });
        }
    }
}