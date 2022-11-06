//Watermark parallaxing on scroll
function netafimParallax() {
    var scrollAmount = $(window).scrollTop();
    var windowsize = $(window).width();
    //Only parallax on tablet landscape and higher
    if (windowsize >= 980) {
        $('.has-parallax').each(function () {
            var scrolled = (($(this).offset().top - scrollAmount) * 0.25) + 'px';
            $(this).css({ "transform": "translate(0," + scrolled + ")" });
        });
    }
    else {
        $('.has-parallax').each(function () {
            $(this).css({ "transform": "translate(0px,0px)" });
        });
    }
}

function verticalTextScroll() {
    if ($('.vertical-text').is(":visible")) {
        var scrollAmount = $(window).scrollTop();
        $('.vertical-text').each(function () {
            var this_container = $(this).parents('.component');
            var this_container_height = this_container.height();
            $(this).css("opacity", 1 - ((scrollAmount - (this_container.offset().top + this_container_height - 490)) / 200));
            if ((scrollAmount + 90 > this_container.offset().top) && (scrollAmount < this_container.offset().top + this_container_height - 300)) {
                $(this).addClass('fixed-top');
            } else {
                $(this).removeClass('fixed-top');
            }
        });
    }
}

$(window).on('scroll', function () {
    netafimParallax();
    verticalTextScroll();
});
$(window).on('resize', function () {
    netafimParallax();
});