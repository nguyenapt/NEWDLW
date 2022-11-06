generateMobileNav();
$('.nav-search').on('click', function () {
    $(this).toggleClass('clicked');
    $('.nav-search-container').toggleClass('show-search');
    $('nav').removeClass('show-nav');
    $('body').removeClass('no-scroll');
    $('.nav-content').removeClass('show-nav-content');
});

var uagent = navigator.userAgent.toLowerCase();

//Only generate desktop menu from ipad
var uagent = navigator.userAgent.toLowerCase();
if ($('.hero-banner').is(':visible')) {
    $('body').addClass('transparent-header');
}
//Only generate desktop menu from ipad
if (uagent.search("ipad") > -1 || uagent.search("mobi") <= -1) {
    generateDesktopNav();
}
$(document).on('scroll', function () {
    var st = $(this).scrollTop();
    if (st >= 36) {
        $('nav').addClass('sticky');
        $('body').removeClass('transparent-header');
    } else {
        $('nav').removeClass('sticky');
        if ($('.hero-banner').is(':visible')) {
            $('body').addClass('transparent-header');
        }
    }
});
$(document).on('touchmove', function (event) {
    if (window.scrollY >= 36) {
        $('nav').addClass('sticky');
        $('body').removeClass('transparent-header');
    } else {
        $('nav').removeClass('sticky');
        if ($('.hero-banner').is(':visible')) {
            $('body').addClass('transparent-header');
        }
    }
});
checkDoormatFlyoutPos();

function checkDoormatFlyoutPos() {
    $('.doormat-with-thumbnail, .doormat-three-col').each(function () {
        if ($(this).parent().offset().left > 1100) {
            var translateValue = $(this).width() - $(this).parent().width() + 120;
            $(this).css('transform', 'translateX(-' + translateValue + 'px)');
            $(this).css({ 'left': '', 'right': '', 'margin-left': '', 'margin-right': '' });
        } else {
            $(this).css('transform', 'translateX(0)');
            $(this).css({ 'left': 0, 'right': 0, 'margin-left': 'auto', 'margin-right': 'auto' });
        }
    });
}

function setBreadCrumbBg() {
    var breadcrumb = $('.breadcrumb').first().parents('.component');
    var breadcrumb_next = breadcrumb.next('div').find('section.component').first();
    if (breadcrumb_next.hasClass('gray-bg')) {
        breadcrumb.addClass('gray-bg');
    }
}
setBreadCrumbBg();


function generateDesktopNav() {
    //Main nav
    var html_nav_content_primary = '';
    $('.nav-content-primary .a1').each(function () {
        var activeClass = '';
        if ($(this).parent().hasClass('active')) {
            activeClass = "active";
        }
        html_nav_content_primary += '<li class="l1 ' + activeClass + '">' + $(this).clone().wrap('<div/>').parent().html();
        if ($(this).hasClass('has-arrow')) {
            html_nav_content_primary += '<div class="doormat-dropdown ' + $(this).data('doormat-class') + '">';
            switch ($(this).data("doormat-class")) {
                case "doormat-three-col":
                    html_nav_content_primary += '<a href="' + $(this).attr('href') + '"><h2>' + $(this).text() + '</h2></a>';
                    html_nav_content_primary += '<div class="doormat-inner-flex">';

                    // 1st column, only .with-image
                    if ($(this).next('.sub-item-container').find('.col1').length > 0) {
                        html_nav_content_primary += '<div class="doormat-col"><ul>';
                        $(this).next('.sub-item-container').find('.col1').each(function () {
                            if ($(this).hasClass('with-image')) {
                                html_nav_content_primary += '<li><a href="' + $(this).attr('href') + '"><img alt="" src="' + $(this).data("img") + '"></a><a href="' + $(this).attr('href') + '" class="general-btn">' + $(this).text() + '</a></li>';
                            } else {
                                html_nav_content_primary += '<h3>' + $(this).text() + '</h3>';
                                html_nav_content_primary += $(this).next('.sub-item-container').find('ul').clone().wrap('<div/>').html();
                            }
                        });
                        html_nav_content_primary += '</ul></div>';
                    }

                    // 2nd column, w/w-o h4 sub-heading
                    if ($(this).next('.sub-item-container').find('.col2.has-arrow').length > 0) {
                        html_nav_content_primary += '<div class="doormat-col">';
                        if ($(this).next('.sub-item-container').find('.col2.has-arrow').attr("href") != "") {
                            html_nav_content_primary += '<a href="' + $(this).next('.sub-item-container').find('.col2.has-arrow').attr("href") + '"><h3>' + $(this).next('.sub-item-container').find('.col2.has-arrow').text() + '</h3></a>';
                        } else {
                            html_nav_content_primary += '<h3>' + $(this).next('.sub-item-container').find('.col2.has-arrow').text() + '</h3>';
                        }
                        html_nav_content_primary += $(this).next('.sub-item-container').find('.col2.has-arrow').next('.sub-item-container').find('ul').clone().wrap('<div/>').parent().html();
                        html_nav_content_primary += '</div>';
                    }

                    //3rd col
                    if ($(this).next('.sub-item-container').find('.col3').length > 0) {
                        if ($(this).next('.sub-item-container').find('.col3.has-arrow').length > 0) {
                            html_nav_content_primary += '<div class="doormat-col">';
                            if ($(this).next('.sub-item-container').find('.col3.has-arrow').attr("href") != '') {
                                html_nav_content_primary += '<a href="' + $(this).next('.sub-item-container').find('.col3.has-arrow').attr("href") + '"><h3>' + $(this).next('.sub-item-container').find('.col3.has-arrow').text() + '</h3></a>';
                            } else {
                                html_nav_content_primary += '<h3>' + $(this).next('.sub-item-container').find('.col3.has-arrow').text() + '</h3>';
                            }

                            html_nav_content_primary += $(this).next('.sub-item-container').find('.col3.has-arrow').next('.sub-item-container').find('ul').clone().wrap('<div/>').parent().html();
                            html_nav_content_primary += '</div>';
                        } else {
                            html_nav_content_primary += '<div class="doormat-col">';
                            html_nav_content_primary += '<h3>' + $(this).next('.sub-item-container').find('.col3').text() + '</h3>';
                            html_nav_content_primary += '</div>';
                        }
                    }
                    html_nav_content_primary += '</div>';
                    break;
                case "doormat-one-col":
                    html_nav_content_primary += '<div class="doormat-inner">';
                    if ($(this).attr("href") != '') {
                        html_nav_content_primary += '<a href="' + $(this).data("href") + '"><h2>' + $(this).text() + '</h2></a>';
                    } else {
                        html_nav_content_primary += '<h2>' + $(this).text() + '</h2>';
                    }
                    html_nav_content_primary += $(this).next('.sub-item-container').children('ul').clone().wrap('<div/>').parent().html();
                    html_nav_content_primary += '</div>';
                    break;
                case "doormat-with-thumbnail":
                    html_nav_content_primary += '<div class="doormat-inner">';
                    if ($(this).attr("href") != '') {
                        html_nav_content_primary += '<a href="' + $(this).attr("href") + '"><h2>' + $(this).text() + '</h2></a>';
                    } else {
                        html_nav_content_primary += '<h2>' + $(this).text() + '</h2>';
                    }
                    html_nav_content_primary += '<ul>';
                    $(this).next('.sub-item-container').find('.a2').not('.view-all').each(function () {
                        if ($(this).data('img') !== "" && !$(this).hasClass('.view-all')) {
                            html_nav_content_primary += '<li><a href="' + $(this).attr('href') + '"><img alt="' + $(this).text() + '" src="' + $(this).data('img') + '"></a>';
                            html_nav_content_primary += '<a href="' + $(this).attr('href') + '"><h4>' + $(this).text() + '</h4></a></li>';
                        }
                    });
                    html_nav_content_primary += '</ul>';
                    html_nav_content_primary += '<a class="general-btn" href="' + $(this).next('.sub-item-container').find('.view-all').attr('href') + '">' + $(this).next('.sub-item-container').find('.view-all').text() + '</a></div>';

                    break;
            }

            html_nav_content_primary += '</div>';
        }

        html_nav_content_primary += '</li>';
    });
    $('.desktop-nav-primary').html(html_nav_content_primary);
    //$(html_nav_content_primary).appendTo('.desktop-nav-primary');

    $('.doormat-one-col').each(function () {
        var translateValue = 300 - $(this).parent().width();
        $(this).css('transform', 'translateX(calc(-' + translateValue + 'px))');
    });

    //Secondary nav
    var html_nav_content_secondary = '';
    $('.nav-content .nav-social a').each(function () {
        html_nav_content_secondary += '<li class="social-link">' + $(this).clone().wrap('<div/>').parent().html() + '</li>';
    });
    html_nav_content_secondary += $('.nav-content-secondary').html();
    $(html_nav_content_secondary).appendTo('.desktop-nav-secondary');
}

function generateMobileNav() {
    $('.nav-content-primary, .nav-content-secondary').find('.has-arrow').each(function () {
        var newElement = '<span class="hidden-arrow"></span>';
        $(this).append(newElement);
    });
}

//Trigger & navigate the main menu on mobile
$('.nav-menu').on('click', function () {
    $('body').toggleClass('no-scroll');
    $('nav').toggleClass('show-nav');
    $('.nav-content').toggleClass('show-nav-content');
    $('.nav-search-container').removeClass('show-search');
    $('.nav-search').removeClass('clicked');
});
$('.nav-content a.has-arrow .hidden-arrow').on('click', function (e) {
    if ($(this).parent().data("open") != 'popup-overlay') {
        e.preventDefault();
    }
    if ($(this).parent().next().is('.sub-item-container')) {
        if ($(this).parent().parent().hasClass('l2')) {
            $(this).parent().next('.sub-item-container').show();
            $('.nav-content-inner').css('transform', 'translateX(-200%)');
        } else {
            $(this).parent().next('.sub-item-container').show();
            $('.nav-content-inner').css('transform', 'translateX(-100%)');
        }
    }
});
$('.btn-back').on('click', function () {
    if ($(this).hasClass('back-to-l1')) {
        $('.nav-content-inner').css('transform', 'translateX(0)');
        $('.nav-content-inner').css('height', 'auto');
        $(this).parent().hide();
    } else if ($(this).hasClass('back-to-l2')) {
        $(this).parent().hide();
        $('.nav-content-inner').css('transform', 'translateX(-100%)');
    }
});

// Open overlay
$('a[data-open="popup-overlay"]').on('click', function (e) {
    $('body').addClass('no-scroll');

    var popupId = $(this).attr("data-popup-id");
    $('.popup-overlay[data-popup-id="' + popupId + '"]').addClass('show-popup');
    e.preventDefault();
});

// Close overlay
$('.popup-overlay .btn-close').on('click', function () {
    $('body').removeClass('no-scroll');
    $('.popup-overlay').removeClass('show-popup');
    $('nav').removeClass('show-nav');
    $('.nav-content').removeClass('show-nav-content');
});

//Other industries
$('.desktop-nav-secondary a[data-open="industry-selection"]').on('click', function () {
    $(this).next('.sub-item-container').toggleClass('show-dropdown-industry');
});

$(document).mouseup(function (e) {
    if ($('.desktop-nav-secondary .sub-item-container').is(":visible") && !$(e.target).hasClass('.show-dropdown-industry')) {
        $('.show-dropdown-industry').removeClass('show-dropdown-industry');
    }

    if (!$(e.target).parents().addBack().is('.show-doormat') && !$(e.target).is('.a1.clicked')) {
        $('.show-doormat').removeClass('show-doormat');
        $('.a1.clicked').removeClass('clicked');
    }
});

// Select all links with hashes
$('a[href*="#"]').not('[href="#"]').not('[href="#0"]').on('click', function (event) {
    // On-page links
    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
        // Figure out element to scroll to
        var target = $(this.hash);
        target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
        // Does a scroll target exist?
        if (target.length) {
            // Only prevent default if animation is actually gonna happen
            event.preventDefault();
            $('html, body').animate({
                scrollTop: target.offset().top
            }, 1000, function () {
                // Callback after animation
                // Must change focus!
                var $target = $(target);
                $target.focus();
                if ($target.is(":focus")) { // Checking if the target was focused
                    return false;
                } else {
                    $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                    $target.focus(); // Set focus again
                };
            });
        }
    }
});

//Function to detect element is in viewport
$.fn.isInViewport = function () {
    var elementTop = $(this).offset().top;
    var elementBottom = elementTop + $(this).outerHeight();
    var viewportTop = $(window).scrollTop() - 200;
    var viewportBottom = viewportTop + $(window).height();
    return elementBottom > viewportTop && elementTop < viewportBottom;
};