// Form date 1
if (jQuery().datepicker) {
    jQuery('#checkin').datepicker({
        showAnim: "drop",
        dateFormat: "dd/mm/yy",
        minDate: "-0D",
        // onSelect: function() {
        //     setdate('#checkin','from');
        // }
    });
    jQuery('.datepicker').datepicker({
        showAnim: "drop",
        dateFormat: "dd/mm/yy",
        minDate: "-0D",
        // onSelect: function() {
        //     setdate('#checkin','from');
        // }
    });
}
// Form date 1
if (jQuery().datepicker) {
    jQuery('#checkin2', '.datepicker2').datepicker({
        showAnim: "drop",
        dateFormat: "dd/mm/yy",
        minDate: "-0D",
        // onSelect: function() {
        //     setdate('#checkin','from');
        // }
    });
}
// Chọn show modal
//$(".click_view").click(function() {
//    var selector = $(this).data('modalid');
//    $.ajax({
//        url: '/check-login',
//        method: "POST",
//        data: null,
//        dataType: "json",
//        contentType: 'application/json; charset=utf-8',
//        success: function(data) {
//            if (typeof selector != 'undefined' && selector != null) {
//                $(selector).modal();
//            }
//            closeLoader();
//        }
//    })
//});
// Giá VND
Array.from(document.getElementsByClassName('money')).forEach(money => {
        money.innerHTML += 'đ';
    })
    //  Tat iframe
$("#close").click(function() {
    $(".view_iframe").css({ "display": 'none' });
});
// Sticky menu //
$(document).ready(function() {
    var w = window.innerWidth;
    if (w > 0) {
        $(() => {

            //On Scroll Functionality
            $(window).scroll(() => {
                var windowTop = $(window).scrollTop();
                windowTop > 10 ? $('#nav').addClass('navShadow_1') : $('#nav').removeClass('navShadow_1');
                windowTop > 10 ? $('ul').css('top', '100px') : $('ul').css('top', '160px');
            });

        });
    }
})


// remove  popup
$(document).ready(function() {
    $(".lang_show").click(function() {
        $(".show_lang").toggle(200);
    });
});

//Menu header
jQuery(document).ready(function($) {
    $('.menu').responsiveMenu({
        breakpoint: '1100'
    });
});

// Facebook chat show hide
(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s);
    js.id = id;
    js.src = "//connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v2.5";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

jQuery(document).ready(function() {
    jQuery(".chat_fb").click(function() {
        jQuery('.fchat').toggle('slow');
    });
});
