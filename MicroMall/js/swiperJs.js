var nav = null;
var mySwiper = null;
var pages = null;
var swiperGallery = null;

function start() {
    nav = $('.swiper-nav').swiper({
        slidesPerView: 'auto',
        freeMode: true,
        noSwiping: true,
        freeModeFluid: true,
        calculateHeight: true,
        visibilityFullFit: true,
        onSlideClick: function (nav) {
            pages.swipeTo(nav.clickedSlideIndex)
        }
    });
    mySwiper = new Swiper('#banner', {
        loop: true,
        autoplay: 5000,
        calculateHeight: true,
        pagination: '.pagination',

        //其他设置
    });
    pages = $('.swiper-pages').swiper({
        noSwiping: true,
        onSlideChangeStart: function () {
            $(".swiper-nav .active").removeClass('active')
            $(".swiper-nav .swiper-slide").eq(pages.activeIndex).addClass('active')
        },
    });
    $('.scroll-container').each(function () {
        $(this).swiper({

            mode: 'vertical',
            scrollContainer: true,
            mousewheelControl: true,
            scrollbar: {
                container: $(this).find('.swiper-scrollbar')[0]
            }
        })
    });
    swiperGallery = $('.swiper-gallery').swiper({
        mode: 'vertical',
        slidesPerView: 'auto',
        freeMode: true,
        freeModeFluid: true,
        scrollbar: {
            container: $('.swiper-gallery .swiper-scrollbar')[0]
        }
    });
    fixPagesHeight();
}

function fixPagesHeight() {
    $('.swiper-pages').css({
        height: $(window).height() - nav.height - 48
    })
}
$(window).on('resize', function () {
    fixPagesHeight()
})