function modal_initialization(id_tag, width){
	var _modal = '<div class='modal fade" id="'+id_tag+'" tabindex="-1" role="dialog" aria-labelledby="'+id_tag+'Title" aria-hidden="true">';
	    _modal += '<div class='modal-dialog" role="document" style="width: '+width+'">';
	        _modal += '<div class='modal-content">';
	            _modal += '<div class='modal-header">';
	                _modal += '<h5 class='modal-title" id="'+id_tag+'Title">Title</h5>';
	                _modal += '<button type="button" class='close" data-dismiss="modal" aria-label="Close" style="margin-top: -22px;">';
	                    _modal += '<span aria-hidden="true">&times;</span>';
	                _modal += '</button>';
	            _modal += '</div>';
	            _modal += '<div class='modal-body" id="'+id_tag+'Body"></div>';
	            /*_modal += '<div class='modal-footer">';
	                _modal += '<button type="button" class='btn btn-secondary" data-dismiss="modal">Close</button>';
	                _modal += '<button type="button" class='btn btn-primary">Save changes</button>';
	            _modal += '</div>';*/
	        _modal += '</div>';
	    _modal += '</div>';
	_modal += '</div>';

	$('html').append(_modal);
}

function modal_open(id_tag, title, href){
	$('#'+id_tag+'Title').html(title);
	$.ajax({
	    url     : href,
	    dataType: 'html',
	    type    : 'GET',
	    async   : false,
	    data    : {},
	    success : function(response, status, jqXHR){
	      	$('#'+id_tag+'Body').html(response);
	    },
	    error   : function(jqXHR, status, err){
	    },
	    complete: function(jqXHR, status){
	    }
  	});
	$('#'+id_tag).modal();
}

function modal_close(id_tag){
	$('#'+id_tag).modal('hide');
	$('#'+id_tag+'Title').html('');
	$('#'+id_tag+'Body').html('');
}

$(function(){
	$('body').on('click', '.sysModal', function (e) {
		e.preventDefault();
        _i = $(this);
        _modal=$(this).attr('modal');
		modal_open(_modal, _i.attr('title'), _i.attr('href'));
	});
});