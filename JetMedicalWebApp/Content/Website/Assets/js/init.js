function $$$(id) {
	return document.getElementById(id);
}
function	Forward(url) {
	window.location.href = url;
}
function	_postback() {
	return void(1); 
}
//----------------------------------------------------------------------------------------------------------------------
function ajaxFunction() {
	var xmlHttp=null;
	try {
		// Firefox, Internet Explorer 7. Opera 8.0+, Safari.
		xmlHttp = new XMLHttpRequest();
	}
	catch (e) {
		// Internet Explorer 6.
		try {
			xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
		}
		catch (e) {
			try{
				xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
			}
			catch (e) {
				return false;
			}
		}
	}
}



//----------------------------------------------------------------------------------------------------------------------
/** Code chạy send mail */
function $query(obj) {
	var query = "";
	$(obj).find("input").each(function(i){
		var t = $(obj).find("input").eq(i);
		if ((t.attr("type") != "checkbox") && (t.attr("type") != "button") && (t.attr("type") != "radio") ) {
			if (t.attr("type") != "password") {
				query += "&"+t.attr("name")+"="+encodeURIComponent(t.val());
			} else {
				query += "&"+t.attr("name")+"="+document.getElementById(t.attr("name")).value;
			}
		}
		else {
			if(t.attr("type") == "checkbox") {
				if (t.is(":checked"))
					query += "&"+t.attr("name")+"="+t.attr("value");
			}
			else if (t.attr("type") == "radio") {
				if (t.is(":checked"))
					query += "&"+t.attr("name")+"="+t.attr("value");
			}
		}
	});
	$(obj).find("textarea").each(function(i) {
		var t = $(obj).find("textarea").eq(i);
		query += "&"+t.attr("name")+"="+encodeURIComponent(t.val());
	});
	$(obj).find("select").each(function(i) {
		var t = $(obj).find("select").eq(i);
		query += "&"+t.attr("name")+"="+encodeURIComponent(t.val());
	});

	return query.substring(1);
}
function $query_unt(obj) {
	var query = "";
	$(obj).find("input").each(function(i){
		var t = $(obj).find("input").eq(i);
		if((t.attr("type") != "button") && (t.attr("type") != "submit") && (t.attr("type") != "reset") && (t.attr("type") != "hidden")) {
			if ((t.attr("type") != "checkbox") && (t.attr("type") != "radio") ) {
				t.val('');
			} else {
				t.attr("checked", false);
			}
		} else {}
	});
	$(obj).find("textarea").each(function(i) {
		var t = $(obj).find("textarea").eq(i);
		t.val('');
	});
	return true;
}
function showLoader() {
	$("#_loading").html("<div style=\"position: fixed; top: 50%; right: 50%; text-align: center; background: transparent; z-index: 999999999;\"><div class=\"windows8\"> <div class=\"wBall\" id=\"wBall_1\"> <div class=\"wInnerBall\"> </div> </div> <div class=\"wBall\" id=\"wBall_2\"> <div class=\"wInnerBall\"> </div> </div> <div class=\"wBall\" id=\"wBall_3\"> <div class=\"wInnerBall\"> </div> </div> <div class=\"wBall\" id=\"wBall_4\"> <div class=\"wInnerBall\"> </div> </div> <div class=\"wBall\" id=\"wBall_5\"> <div class=\"wInnerBall\"> </div> </div> </div></div>").hide().fadeIn(10);
	block = true;
}
function closeLoader() {
	$("#_loading").html("").hide().fadeOut(100);
	block = false;
}
function showResult(type,data) {
	closeLoader();
	$("#"+type+"").html(data).hide().fadeIn(100);
	block = false;
}
// End send mail 

//----------------------------------------------------------------------------------------------------------------------
function sendMail(type, id) {
	var dataList = $query('#'+id);
	showLoader();
	$.ajax({
		url:'/action.php',
		type: 'POST',
		data: 'url='+type+'&'+dataList,
		dataType: "html",
		success: function(data){
			closeLoader();
			$query_unt('#'+id);
			alert(data);
		}
	});
	return false;
}
function sendRegEmail() {
	var email = document.forms['email_register']['email'].value;
	var email_filter=/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
	var test = true;
	var lang = document.forms['email_register']['lang'].value;
	var thongbao = document.forms['email_register']['thongbao'].value;
	test = email_filter.test(email);
	if(test==false) {
		alert(thongbao);
		return false;
	} else {
		showLoader();
		$.ajax({ 
			url:'/action.php',
			type: 'POST',
			data: 'url=register_email&email='+email+'&lang='+lang,
			dataType: "html",
			success: function(data){
				closeLoader();
				$('#_reg_email').val('');
				alert(data);
			}
		});
	}
	return false;
}
function sendOrder(type, id) {
	var dataList = $query('#'+id);
	showLoader();
	$.ajax({
		url:'/action.php',
		type: 'POST',
		data: 'url='+type+'&'+dataList,
		dataType: "html",
		success: function(data){
			closeLoader();
			$query_unt('#'+id);
			alert(data, function() {
				window.location.reload();
			});
			//alert(data);
		}
	});
	return false;
}
function addgh(type, id) {
    var dataList = $query('#'+id);
    showLoader();
    $.ajax({
        url:'/action.php',
        type: 'POST',
        data: 'url='+type+'&'+dataList,
        dataType: "json",
        success: function(data){
            closeLoader();
            alert(data.msg);
            $('.coucartabc').text(data.num);
        }
    });
    return false;
}
// --------------------------------------------------------------------------------------------------------------------
function check_order(){
	var input = document.forms['frm_order'].getElementsByTagName('input');
	var txtarea = document.forms['frm_order'].getElementsByTagName('textarea');
	var err_field = $('#txtEnterField').val();
	var err_email = $('#txtEnterMail').val();
	var ht = $('#hoten').val();
	var nd = $('#noidung').val();

	var inputs = new Array();
	for(var i=0; i<(input.length+txtarea.length); i++){
		if(i<input.length) inputs[i]=input[i];
		else inputs[i]=txtarea[i-input.length];
	}
	var run_onchange = false;
	var emailfilter=/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
	var pass ='';
	function valid(){
		var errors = false;
		for(var i=0; i<inputs.length; i++){
			var value = inputs[i].value;
			var id = inputs[i].getAttribute('id');

			var span = document.createElement('span');
			var p = inputs[i].parentNode;
			if(p.lastChild.nodeName == 'SPAN') {p.removeChild(p.lastChild);}

			if( value == ''){
				if(id == 'hoten') {
					span.innerHTML = ht;
				}
				else if(id == 'sodienthoai') {
					span.innerHTML = sdt;
				}
				else if(id == 'diachi') {
					span.innerHTML = dc;
				}
				else if(id == 'email') {
					span.innerHTML = 'Email';
				}
				else if(id == 'content') {
					span.innerHTML = nd;
				}
				else if(id != '_send_order') {
					span.innerHTML = err_field;
				}
			}
			if(id == 'email' && value != '') {
				var returnval=emailfilter.test(value);
				if(returnval==false){span.innerHTML = err_email;}
			}
			if(id == 'sodienthoai' && value != ''){
				if(isNaN(value) == true || value.indexOf('.')!=-1 || value < 0){span.innerHTML = err_tell;}
				if(isNaN(value) == false && value.length < 10){span.innerHTML = err_tell;}
			}

			if(span.innerHTML != ''){
				inputs[i].parentNode.appendChild(span);
				span.setAttribute('class', 'show-error');
				errors = true;
				run_onchange = true;
				inputs[i].style.border = '1px solid #ff6e69';
			}
		}
		return !errors;
	}

	var register = document.getElementById('_send_order');
	register.onclick = function(){
		return valid();
	}

	for(var i=0; i<inputs.length; i++){
		var id = inputs[i].getAttribute('id');
		inputs[i].onchange = function(){
			if(run_onchange == true){
				this.style.border = '1px solid #e0e1e0';
				valid();
			}
		}
	}
}

//----------------------------------------------------------------------------------------------------------------------
function check_contact(){
	var input = document.forms['frm_contact'].getElementsByTagName('input');
	var txtarea = document.forms['frm_contact'].getElementsByTagName('textarea');
	var err_field = $("[name='lang_field']").val();
	var err_email = $("[name='lang_email']").val();
	var err_phone = $("[name='lang_phone']").val();
	var ht = $('#hoten1').val();
	var sdt = $('#sodienthoai1').val();
	var dc = $('#diachi1').val();
	var nd = $('#noidung1').val();

	var inputs = new Array();
	for(var i=0; i<(input.length+txtarea.length); i++){
		if(i<input.length) inputs[i]=input[i];
		else inputs[i]=txtarea[i-input.length];
	}
	var run_onchange = false;
	var email_filter=/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
	var pass ='';
	function valid(){
		var errors = false;
		for(var i=0; i<inputs.length; i++){
			var value = inputs[i].value;
			var name = inputs[i].getAttribute('name');

			var span = document.createElement('span');
			var p = inputs[i].parentNode;
			if(p.lastChild.nodeName == 'SPAN') {p.removeChild(p.lastChild);}

			if( value == ''){
				if(name == 'full_name') {
					span.innerHTML = ht;
				}
				else if(name == 'email') {
					span.innerHTML = ht;
				}
				else if(name == 'add') {
					span.innerHTML = dc;
				}
				else if(name != 'tell' && name != 'content'  && name != 'company'  && name != 'send_contact') {
					span.innerHTML = err_field;
				}
			}
			if(name == 'email' && value != '') {
				var return_val=email_filter.test(value);
				if(return_val==false){span.innerHTML = err_email;}
			}
			
			if(span.innerHTML != ''){
				inputs[i].parentNode.appendChild(span);
				span.setAttribute('class', 'show-error');
				errors = true;
				run_onchange = true;
				inputs[i].style.border = '1px solid #ff6e69';
			}
		}
		return !errors;
	}
	var register = document.getElementById('_send_contact');
	register.onclick = function(){
		return valid();
	}
	for(var i=0; i<inputs.length; i++){
		var id = inputs[i].getAttribute('id');
		inputs[i].onchange = function(){
			if(run_onchange == true){
				this.style.border = '1px solid #cecfce';
				valid();
			}
		}
	}
}