function enCodeURL(id, donViId, isShowLoading, waitingElementOption) {
    var defered = $.Deferred();
    var data = {
        id: id,
        donViId: donViId
    }
    if (isShowLoading == true) {
        if (!waitingElementOption) {
            waitingElementOption = {
                selector: 'body'
            };
        }

        run_waitMe(waitingElementOption.selector);
    }

    $.ajax({
        url: '/EncodeAndDecode/EncodeURL',
        type: "POST",
        data: data != null ? JSON.stringify(data) : null,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        error: function () {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }

            defered.reject('Không tìm thấy máy chủ.');

            return false;
        },
        success: function (res) {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }

            defered.resolve(res);
        }
    });

    return defered.promise();
}

function sendDataToURL(url, data, isShowLoading, waitingElementOption) {
    var defered = $.Deferred();

    if (isShowLoading == true) {
        if (!waitingElementOption) {
            waitingElementOption = {
                selector: 'body'
            };
        }
        run_waitMe(waitingElementOption.selector);
    }

    $.ajax({
        url: url,
        type: "POST",
        data: data != null ? JSON.stringify(data) : null,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        error: function () {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }

            defered.reject('Không tìm thấy máy chủ.');

            return false;
        },
        success: function (res) {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }

            defered.resolve(res);
        }
    });

    return defered.promise();
}

function sendDataFileToURL(url, data, isShowLoading, waitingElementOption) {
    var defered = $.Deferred();

    if (isShowLoading == true) {
        if (!waitingElementOption) {
            waitingElementOption = {
                selector: 'body'
            };
        }
        run_waitMe(waitingElementOption.selector);
    }

    $.ajax({
        url: url,
        type: "POST",
        data: data,
        processData: false,
        contentType: false,
        error: function (xhr, status, p3, p4) {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }
            //stop_waitMe(waitingElementOption.selector, waitingElementOption.option);

            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);

            defered.reject('Không tìm thấy máy chủ.');

            return false;
        },
        success: function (res) {
            if (isShowLoading == true) {
                stop_waitMe(waitingElementOption.selector);
            }

            defered.resolve(res);
        }
    });

    return defered.promise();
}

function run_waitMe(selector) {
    if (!selector) {
        selector = 'body';
    }
    $(selector).block({
        message: '<h2><i class="fa fa-spinner fa-spin vd_green"></i></h2>',
        css: {
            border: 'none',
            padding: '15px',
            background: 'none',
        },
        overlayCSS: { backgroundColor: '#FFF' }
    });
}

function stop_waitMe(selector) {
    if (!selector) {
        selector = 'body';
    }
    $(selector).find('.blockUI')
        .fadeOut(300, function () { $(this).remove(); });
}


function formatVND(n, currency) {
    return currency + " " + n.toFixed(0).replace(/./g, function (c, i, a) {
        return i > 0 && c !== "," && (a.length - i) % 3 === 0 ? "." + c : c;
    });
}

function dinhDangTien(n) {
    var strTien = n.toString().split('.');
    return parseFloat(strTien[0]).toString().replace(/./g, function (c, i, a) {
        return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
    }) + (strTien.length > 1 ? '.' + strTien[1] : '');
}

function isNullOrEmpty(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
}

function isNullOrEmptySelect2(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null || value == 'null' || value == "-1";
}

// Đọc số sang chữ
var mangso = ['không', 'một', 'hai', 'ba', 'bốn', 'năm', 'sáu', 'bảy', 'tám', 'chín'];
function dochangchuc(so, daydu) {
    var chuoi = "";
    chuc = Math.floor(so / 10);
    donvi = so % 10;
    if (chuc > 1) {
        chuoi = " " + mangso[chuc] + " mươi";
        if (donvi == 1) {
            chuoi += " mốt";
        }
    }
    else if (chuc == 1) {
        chuoi = " mười";

        if (donvi == 1) {
            chuoi += " một";
        }
    }
    else if (daydu && donvi > 0) {
        chuoi = " lẻ";
    }

    if (donvi == 5 && chuc > 1) {
        chuoi += " lăm";
    }
    else if (donvi > 1 || (donvi == 1 && chuc == 0)) {
        chuoi += " " + mangso[donvi];
    }

    return chuoi;
}

function docblock(so, daydu) {
    var chuoi = "";
    tram = Math.floor(so / 100);
    so = so % 100;
    if (daydu || tram > 0) {
        chuoi = " " + mangso[tram] + " trăm";
        chuoi += dochangchuc(so, true);
    }
    else {
        chuoi = dochangchuc(so, false);
    }

    return chuoi;
}

function dochangtrieu(so, daydu) {
    var chuoi = "";
    trieu = Math.floor(so / 1000000); so = so % 1000000;
    if (trieu > 0) {
        chuoi = docblock(trieu, daydu) + " triệu";
        daydu = true;
    }

    nghin = Math.floor(so / 1000);
    so = so % 1000;

    if (nghin > 0) {
        chuoi += docblock(nghin, daydu) + " nghìn"; daydu = true;
    }

    if (so > 0) {
        chuoi += docblock(so, daydu);
    }

    return chuoi;
}

function docTienBangChu(tien) {
    if (tien == 0)
        return mangso[0];

    var chuoi = "",
        hauto = "";

    do {
        ty = tien % 1000000000;
        tien = Math.floor(tien / 1000000000);

        if (tien > 0) {
            chuoi = dochangtrieu(ty, true) + hauto + chuoi;
        }
        else {
            chuoi = dochangtrieu(ty, false) + hauto + chuoi;
        }

        hauto = " tỷ";
    } while (tien > 0);

    return chuoi;
}

var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
var Tien = new Array("", " ngàn", " triệu", " tỷ", " ngàn tỷ", " triệu tỷ");

function DocTienBangChu(SoTien) {
    var lan = 0;
    var i = 0;
    var so = 0;
    var KetQua = "";
    var tmp = "";
    var ViTri = new Array();
    if (SoTien < 0) return "Số tiền âm !";
    if (SoTien == 0) return "Không đồng !";
    if (SoTien > 0) {
        so = SoTien;
    }
    else {
        so = -SoTien;
    }
    if (SoTien > 8999999999999999) {
        //SoTien = 0;
        return "Số quá lớn!";
    }
    ViTri[5] = Math.floor(so / 1000000000000000);
    if (isNaN(ViTri[5]))
        ViTri[5] = "0";
    so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
    ViTri[4] = Math.floor(so / 1000000000000);
    if (isNaN(ViTri[4]))
        ViTri[4] = "0";
    so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
    ViTri[3] = Math.floor(so / 1000000000);
    if (isNaN(ViTri[3]))
        ViTri[3] = "0";
    so = so - parseFloat(ViTri[3].toString()) * 1000000000;
    ViTri[2] = parseInt(so / 1000000);
    if (isNaN(ViTri[2]))
        ViTri[2] = "0";
    ViTri[1] = parseInt((so % 1000000) / 1000);
    if (isNaN(ViTri[1]))
        ViTri[1] = "0";
    ViTri[0] = parseInt(so % 1000);
    if (isNaN(ViTri[0]))
        ViTri[0] = "0";
    if (ViTri[5] > 0) {
        lan = 5;
    }
    else if (ViTri[4] > 0) {
        lan = 4;
    }
    else if (ViTri[3] > 0) {
        lan = 3;
    }
    else if (ViTri[2] > 0) {
        lan = 2;
    }
    else if (ViTri[1] > 0) {
        lan = 1;
    }
    else {
        lan = 0;
    }
    for (i = lan; i >= 0; i--) {
        tmp = DocSo3ChuSo(ViTri[i]);
        KetQua += tmp;
        if (ViTri[i] > 0) KetQua += Tien[i];
        if ((i > 0) && (tmp.length > 0)) KetQua += ',';//&& (!string.IsNullOrEmpty(tmp))
    }
    if (KetQua.substring(KetQua.length - 1) == ',') {
        KetQua = KetQua.substring(0, KetQua.length - 1);
    }
    KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
    return KetQua + " đồng.";//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
}

function DocSo3ChuSo(baso) {
    var tram;
    var chuc;
    var donvi;
    var KetQua = "";
    tram = parseInt(baso / 100);
    chuc = parseInt((baso % 100) / 10);
    donvi = baso % 10;
    if (tram == 0 && chuc == 0 && donvi == 0) return "";
    if (tram != 0) {
        KetQua += ChuSo[tram] + " trăm ";
        if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
    }
    if ((chuc != 0) && (chuc != 1)) {
        KetQua += ChuSo[chuc] + " mươi";
        if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
    }
    if (chuc == 1) KetQua += " mười ";
    switch (donvi) {
        case 1:
            if ((chuc != 0) && (chuc != 1)) {
                KetQua += " mốt ";
            }
            else {
                KetQua += ChuSo[donvi];
            }
            break;
        case 5:
            if (chuc == 0) {
                KetQua += ChuSo[donvi];
            }
            else {
                KetQua += " lăm ";
            }
            break;
        default:
            if (donvi != 0) {
                KetQua += ChuSo[donvi];
            }
            break;
    }
    return KetQua;
}

function readImageURL(input, element) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            element.attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}


function toSlug(str) {
    // Chuyển hết sang chữ thường
    str = str.toLowerCase();

    // xóa dấu
    str = str.replace(/(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)/g, 'a');
    str = str.replace(/(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)/g, 'e');
    str = str.replace(/(ì|í|ị|ỉ|ĩ)/g, 'i');
    str = str.replace(/(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)/g, 'o');
    str = str.replace(/(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)/g, 'u');
    str = str.replace(/(ỳ|ý|ỵ|ỷ|ỹ)/g, 'y');
    str = str.replace(/(đ)/g, 'd');

    // Xóa ký tự đặc biệt
    str = str.replace(/([^0-9a-z-\s])/g, '');
    str = str.replace(/[0-9`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');

    // Xóa khoảng trắng thay bằng ký tự -
    str = str.replace(/(\s+)/g, '-');

    // xóa phần dự - ở đầu
    str = str.replace(/^-+/g, '');

    // xóa phần dư - ở cuối
    str = str.replace(/-+$/g, '');

    // return
    return str;
}


function hienThiPhanBac(length, isShowDot) {
    if (typeof isShowDot == 'undefined') {
        isShowDot = true;
    }

    var space = '';
    if (length > 1) {
        for (var i = 1; i < length; i++) {
            space += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
        }
    }

    if (isShowDot) {
        space += "• ";
    }

    return space;
}

function hienThiPhanBacV2(length) {
    var space = '';

    if (length > 1) {
        for (var i = 0; i < length; i++) {
            if (i == 0) {
                space += '&nbsp;&nbsp;&nbsp;';
            } else {
                space += '-';
            }
        }
    }

    return space;
}


$(function () {
	"use strict";
	new PerfectScrollbar(".header-message-list"), new PerfectScrollbar(".header-notifications-list"), $(".mobile-search-icon").on("click", function() {
		$(".search-bar").addClass("full-search-bar")
	}), $(".search-close").on("click", function() {
		$(".search-bar").removeClass("full-search-bar")
	}), $(".mobile-toggle-menu").on("click", function() {
		$(".wrapper").addClass("toggled")
	}), $(".toggle-icon").click(function() {
		$(".wrapper").hasClass("toggled") ? ($(".wrapper").removeClass("toggled"), $(".sidebar-wrapper").unbind("hover")) : ($(".wrapper").addClass("toggled"), $(".sidebar-wrapper").hover(function() {
			$(".wrapper").addClass("sidebar-hovered")
		}, function() {
			$(".wrapper").removeClass("sidebar-hovered")
		}))
	}), $(document).ready(function() {
		$(window).on("scroll", function() {
			$(this).scrollTop() > 300 ? $(".back-to-top").fadeIn() : $(".back-to-top").fadeOut()
		}), $(".back-to-top").on("click", function() {
			return $("html, body").animate({
				scrollTop: 0
			}, 600), !1
		})
	}), $(function() {
		for (var e = window.location, o = $(".metismenu li a").filter(function() {
				return this.href == e
			}).addClass("").parent().addClass("mm-active"); o.is("li");) o = o.parent("").addClass("mm-show").parent("").addClass("mm-active")
	}), $(function() {
		$("#menu").metisMenu()
	}), $(".chat-toggle-btn").on("click", function() {
		$(".chat-wrapper").toggleClass("chat-toggled")
	}), $(".chat-toggle-btn-mobile").on("click", function() {
		$(".chat-wrapper").removeClass("chat-toggled")
	}), $(".email-toggle-btn").on("click", function() {
		$(".email-wrapper").toggleClass("email-toggled")
	}), $(".email-toggle-btn-mobile").on("click", function() {
		$(".email-wrapper").removeClass("email-toggled")
	}), $(".compose-mail-btn").on("click", function() {
		$(".compose-mail-popup").show()
	}), $(".compose-mail-close").on("click", function() {
		$(".compose-mail-popup").hide()
	}), $(".switcher-btn").on("click", function() {
		$(".switcher-wrapper").toggleClass("switcher-toggled")
	}), $(".close-switcher").on("click", function() {
		$(".switcher-wrapper").removeClass("switcher-toggled")
	}), $("#lightmode").on("click", function() {
		$("html").attr("class", "light-theme")
	}), $("#darkmode").on("click", function() {
		$("html").attr("class", "dark-theme")
	}), $("#semidark").on("click", function() {
		$("html").attr("class", "semi-dark")
	}), $("#minimaltheme").on("click", function() {
		$("html").attr("class", "minimal-theme")
	}), $("#headercolor1").on("click", function() {
		$("html").addClass("color-header headercolor1"), $("html").removeClass("headercolor2 headercolor3 headercolor4 headercolor5 headercolor6 headercolor7 headercolor8")
	}), $("#headercolor2").on("click", function() {
		$("html").addClass("color-header headercolor2"), $("html").removeClass("headercolor1 headercolor3 headercolor4 headercolor5 headercolor6 headercolor7 headercolor8")
	}), $("#headercolor3").on("click", function() {
		$("html").addClass("color-header headercolor3"), $("html").removeClass("headercolor1 headercolor2 headercolor4 headercolor5 headercolor6 headercolor7 headercolor8")
	}), $("#headercolor4").on("click", function() {
		$("html").addClass("color-header headercolor4"), $("html").removeClass("headercolor1 headercolor2 headercolor3 headercolor5 headercolor6 headercolor7 headercolor8")
	}), $("#headercolor5").on("click", function() {
		$("html").addClass("color-header headercolor5"), $("html").removeClass("headercolor1 headercolor2 headercolor4 headercolor3 headercolor6 headercolor7 headercolor8")
	}), $("#headercolor6").on("click", function() {
		$("html").addClass("color-header headercolor6"), $("html").removeClass("headercolor1 headercolor2 headercolor4 headercolor5 headercolor3 headercolor7 headercolor8")
	}), $("#headercolor7").on("click", function() {
		$("html").addClass("color-header headercolor7"), $("html").removeClass("headercolor1 headercolor2 headercolor4 headercolor5 headercolor6 headercolor3 headercolor8")
	}), $("#headercolor8").on("click", function() {
		$("html").addClass("color-header headercolor8"), $("html").removeClass("headercolor1 headercolor2 headercolor4 headercolor5 headercolor6 headercolor7 headercolor3")
	})
	
	
	
	// sidebar colors 


    $('#sidebarcolor1').click(theme1);
    $('#sidebarcolor2').click(theme2);
    $('#sidebarcolor3').click(theme3);
    $('#sidebarcolor4').click(theme4);
    $('#sidebarcolor5').click(theme5);
    $('#sidebarcolor6').click(theme6);
    $('#sidebarcolor7').click(theme7);
    $('#sidebarcolor8').click(theme8);

    function theme1() {
      $('html').attr('class', 'color-sidebar sidebarcolor1');
    }

    function theme2() {
      $('html').attr('class', 'color-sidebar sidebarcolor2');
    }

    function theme3() {
      $('html').attr('class', 'color-sidebar sidebarcolor3');
    }

    function theme4() {
      $('html').attr('class', 'color-sidebar sidebarcolor4');
    }
	
	function theme5() {
      $('html').attr('class', 'color-sidebar sidebarcolor5');
    }
	
	function theme6() {
      $('html').attr('class', 'color-sidebar sidebarcolor6');
    }

    function theme7() {
      $('html').attr('class', 'color-sidebar sidebarcolor7');
    }

    function theme8() {
      $('html').attr('class', 'color-sidebar sidebarcolor8');
    }
	
	
	
	
});

function notification(position, type, icon, title, message) {
    if (typeof icon == 'undefined' || icon == '') {
        icon = 'bx bx-x-circle';
    }
    if (typeof position == 'undefined' || position == '') {
        position = 'top right';
    }
    if (typeof type == 'undefined' || type == '') {
        type = 'success';
    }

    var notiOptions = {
        pauseDelayOnHover: true,
        rounded: true,
        size: 'mini',
        icon: icon,
        continueDelayOnInactiveTab: false,
        position: position,
        msg: message,
        title: title
    };

    if (typeof title == 'undefined' || title == '') {
        notiOptions.size = 'mini';
    }

    Lobibox.notify(type, notiOptions);

    

    //Lobibox.notify('warning', {
    //    pauseDelayOnHover: true,
    //    continueDelayOnInactiveTab: false,
    //    position: 'top right',
    //    icon: 'bx bx-error',
    //    msg: 'Lorem ipsum dolor sit amet hears farmer indemnity inherent.'
    //});
}



function viewUrl(url) {
    $("#viewUrlModalContent").append('<iframe src="' + url + '" scrolling="no" frameborder="0" id="iframeUrlContent"></iframe>');
    //auto_resize_iframe_timer = window.setInterval("autoresize_frames()", 1000);
    $('#viewUrlModal').modal('show');
}

function autoresize_frames() {
    iframe = document.getElementById("iframeUrlContent");
    if (typeof iframe != 'undefined') {
        var framefenster_size = iframe.contentWindow.document.body.scrollHeight;
        iframe.style.height = framefenster_size + 'px';
    }
}