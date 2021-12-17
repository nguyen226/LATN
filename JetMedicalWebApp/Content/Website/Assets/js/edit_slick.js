$('.slider_doingu').slick({
    slidesToShow: 4,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 1500,
    infinite: true,
    dots: false,
    responsive: [{
            breakpoint: 1025,
            settings: {
                slidesToShow: 3
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 2
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1
            }
        }
    ]
});
$('.slider_doingu_lq').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 1500,
    infinite: true,
    dots: false,
    responsive: [{
            breakpoint: 1025,
            settings: {
                slidesToShow: 3
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 2
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1
            }
        }
    ]
});

$('.slider_danhgia').slick({
    slidesToShow: 2,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 1500,
    dots: true,
    responsive: [{
            breakpoint: 1025,
            settings: {
                arrows: false,
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        }
    ]
});
$('.slider_video').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 3500,
    dots: true,
    responsive: [{
            breakpoint: 1025,
            settings: {
                slidesToShow: 3,
                arrows: false,
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 2,
                arrows: false,
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        }
    ]
});
$('.slide_goikham').slick({
    slidesToShow: 4,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 1500,
    infinite: true,
    dots: false,
    responsive: [{
            breakpoint: 1025,
            settings: {
                slidesToShow: 3,
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 2,
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1
            }
        }
    ]
});
$('.slider_dg').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 5000,
    infinite: true,
    dots: false,
    responsive: [{
            breakpoint: 1025,
            settings: {
                slidesToShow: 2
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 2
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 1
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 1
            }
        }
    ]
});
$('.slider_doitac').slick({
    slidesToShow: 5,
    slidesToScroll: 1,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 1500,
    infinite: true,
    dots: false,
    responsive: [{
            breakpoint: 1025,
            settings: {
                arrows: false,
            }
        },
        {
            breakpoint: 801,
            settings: {
                slidesToShow: 3,
                arrows: false,
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 2,
                arrows: false,
            }
        },
        {
            breakpoint: 481,
            settings: {
                slidesToShow: 2,
                arrows: false,
            }
        }
    ]
});

$('.slider-for').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    fade: true,
    asNavFor: '.slider-nav'
});
$('.slider-nav').slick({
    slidesToShow: 5,
    slidesToScroll: 1,
    asNavFor: '.slider-for',
    dots: false,
    centerMode: true,
    arrows: true,
    focusOnSelect: true
});


$('.slider_brand').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    autoplay: false,
    autoplaySpeed: 1500,
    infinite: true,
    dots: false
});