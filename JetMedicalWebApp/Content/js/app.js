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

