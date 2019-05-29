jQuery(document).ready(function ($) {
    var mainHeader = $('.cd-auto-hide-header'),
        secondaryNavigation = $('.cd-secondary-nav'),
        //this applies only if secondary nav is below intro section
        belowNavHeroContent = $('.sub-nav-hero'),
        headerHeight = mainHeader.height();

    //set scrolling variables
    var scrolling = false,
        previousTop = 0,
        currentTop = 0,
        scrollDelta = 10,
        scrollOffset = 150;

    mainHeader.on('click', '.nav-trigger', function (event) {
        // open primary navigation on mobile
        event.preventDefault();
        mainHeader.toggleClass('nav-open');
    });

    $(window).on('scroll', function () {
        if (!scrolling) {
            scrolling = true;
            (!window.requestAnimationFrame)
                ? setTimeout(autoHideHeader, 100)
                : requestAnimationFrame(autoHideHeader);
        }
    });

    $(window).on('resize', function () {
        headerHeight = mainHeader.height();
    });

    function autoHideHeader() {
        var currentTop = $(window).scrollTop();

        (belowNavHeroContent.length > 0)
             checkStickyNavigation(currentTop); // secondary navigation below intro

        previousTop = currentTop;
        scrolling = false;
    }

    function checkStickyNavigation(currentTop) {
        //secondary nav below intro section - sticky secondary nav
        if (previousTop >= currentTop) {
            //if scrolling up... 
            if (currentTop < secondaryNavigation.height()) {
                //secondary nav is not fixed
                mainHeader.removeClass('is-hidden');
                secondaryNavigation.removeClass('fixed slide-up').addClass('cd-auto-display-header');
                belowNavHeroContent.removeClass('secondary-nav-fixed');
            }
        } else {
            //if scrolling down...	
            if (currentTop > 0) {
                //hide primary nav
                mainHeader.addClass('is-hidden');
                secondaryNavigation.addClass('fixed slide-up').removeClass('cd-auto-display-header');
                belowNavHeroContent.addClass('secondary-nav-fixed');
            }

        }
    }
});