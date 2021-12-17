function showLoader(selector, option) {
    var optionObj = {
        effect: 'rotation',
        text: 'Đang xử lý...',
        bg: 'rgba(255,255,255,0.6)',
        color: '#3333cc',
        sizeW: '32px',
        sizeH: '32px',
        source: 'img.svg',
        onClose: function () { }
    };

    if (option) {
        for (var key in option) {
            optionObj[key] = option[key];
        }
    }

    var html = '<div class="loading-overlay" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: ' + optionObj.bg + '; z-index: 10000;">';
    html +='<div style="text-align: center; width: 100%; position: absolute; top: 40%; margin-top: -50px;">';
    html += '<div class="loader" style="margin-left: auto; margin-right: auto;"></div><div class="word">' + optionObj.text + '</div></div></div></div>'
    
    var overlay = jQuery(html);

    if (!selector) {
        selector = 'body';
    }

    $(selector).append(html);

    //$words = $('.word');

    //$words.each(function () {
    //    var $this = $(this),
    //        ticker = new TickerAnimate($this).reset();
    //    $this.data('ticker', ticker);
    //});

    //overlay.appendTo(document.body);
}
function hideLoader(selector) {
    if (!selector) {
        selector = 'body';
    }

    $(selector + ' .loading-overlay').remove();
}
