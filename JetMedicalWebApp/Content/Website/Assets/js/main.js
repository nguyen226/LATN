jQuery(document).ready(function($){
	//open/close mega-navigation
	$('.cd-dropdown-trigger').on('click', function(event){
		event.preventDefault();
		toggleNav();
	});

	//close meganavigation
	$('.cd-dropdown .cd-close').on('click', function(event){
		event.preventDefault();
		toggleNav();
	});

	//on mobile - open submenu
	$('.has-children').children('a').on('click', function(event){
		//prevent default clicking on direct children of .has-children 
		event.preventDefault();
		var selected = $(this);
		selected.next('ul').removeClass('is-hidden').end().parent('.has-children').parent('ul').addClass('move-out');
	});

	//on desktop - differentiate between a user trying to hover over a dropdown item vs trying to navigate into a submenu's contents
	var submenuDirection = ( !$('.cd-dropdown-wrapper').hasClass('open-to-left') ) ? 'right' : 'left';
	$('.cd-dropdown-content').menuAim({
        activate: function(row) {
        	$(row).children().addClass('is-active').removeClass('fade-out');
        	if( $('.cd-dropdown-content .fade-in').length == 0 ) $(row).children('ul').addClass('fade-in');
        },
        deactivate: function(row) {
        	$(row).children().removeClass('is-active');
        	if( $('li.has-children:hover').length == 0 || $('li.has-children:hover').is($(row)) ) {
        		$('.cd-dropdown-content').find('.fade-in').removeClass('fade-in');
        		$(row).children('ul').addClass('fade-out')
        	}
        },
        exitMenu: function() {
        	$('.cd-dropdown-content').find('.is-active').removeClass('is-active');
        	return true;
        },
        submenuDirection: submenuDirection,
    });

	//submenu items - go back link
	$('.go-back').on('click', function(){
		var selected = $(this),
			visibleNav = $(this).parent('ul').parent('.has-children').parent('ul');
		selected.parent('ul').addClass('is-hidden').parent('.has-children').parent('ul').removeClass('move-out');
	});

	$('.timepicker').timepicker({
	    minuteStep: 1,
	    template: false,
	    showSeconds: true,
	    showMeridian: false,
	});

	function toggleNav(){
		var navIsVisible = ( !$('.cd-dropdown').hasClass('dropdown-is-active') ) ? true : false;
		$('.cd-dropdown').toggleClass('dropdown-is-active', navIsVisible);
		$('.cd-dropdown-trigger').toggleClass('dropdown-is-active', navIsVisible);
		if( !navIsVisible ) {
			$('.cd-dropdown').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend',function(){
				$('.has-children ul').addClass('is-hidden');
				$('.move-out').removeClass('move-out');
				$('.is-active').removeClass('is-active');
			});	
		}
	}

	//IE9 placeholder fallback
	//credits http://www.hagenburger.net/BLOG/HTML5-Input-Placeholder-Fix-With-jQuery.html
	if(!Modernizr.input.placeholder){
		$('[placeholder]').focus(function() {
			var input = $(this);
			if (input.val() == input.attr('placeholder')) {
				input.val('');
		  	}
		}).blur(function() {
		 	var input = $(this);
		  	if (input.val() == '' || input.val() == input.attr('placeholder')) {
				input.val(input.attr('placeholder'));
		  	}
		}).blur();
		$('[placeholder]').parents('form').submit(function() {
		  	$(this).find('[placeholder]').each(function() {
				var input = $(this);
				if (input.val() == input.attr('placeholder')) {
			 		input.val('');
				}
		  	})
		});
	}
});

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

function isNullOrEmpty(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null;
}

function isNullOrEmptySelect2(value) {
    return typeof value == 'string' && !value.trim() || typeof value == 'undefined' || value === null || value == 'null' || value == "-1";
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
            top: '45vh',
            'z-index': 9999999
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

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
