$(document).ready(function () {
    //navbar animation scroll
    var lastScrollTop = 0;
    var $navbar = $('.navbar-fixed-top');
    var animationClass = 'animate__animated animate__slideInDown';

    $(window).scroll(function () {
        var scrollTop = $(this).scrollTop();
        console.log(scrollTop + ">" + lastScrollTop);

        if (scrollTop > 170) {
            $navbar.addClass(animationClass).css('position', 'fixed');
        }
        else {
            $navbar.removeClass(animationClass).css('transform');
            $navbar.removeClass(animationClass).css('position','');
        }
        lastScrollTop = scrollTop;
    });
});