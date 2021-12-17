//var _mindate = new Date();
//var _maxdate = _mindate.addDays(7);

$.extend(
    $.fn.dataTable.defaults, {
        "searching": false,
        "ordering": false,
        "bLengthMenu": false,
        "lengthChange": false
    }
);

function dataTableIni(config) {
    var Type = "";
    if (typeof config.type !== "undefined")
        Type = config.type;

    var scrollX = false;
    if (typeof config.scrollX !== "undefined")
        scrollX = config.scrollX;

    var fixedColumns = {};
    if (typeof config.fixedColumns !== "undefined")
        fixedColumns = config.fixedColumns;

    var rowsGroup = [];
    if (typeof config.rowsGroup !== "undefined")
        rowsGroup = config.rowsGroup;

    $('#' + config.idTable + ' thead th').each(function () {
        var title = $(this).text();
        var _name = $(this).attr("name");
        var _placeholder = $(this).attr("placeholder");
        var _option = $(this).attr("option");
        switch (_option) {
            case "none":
                //$(this).html('');
                break;
            case "date":
                $(this).html('<input class='TableFieldFilter' + Type + ' TableFieldFilterDate" type="text" format="' + _option + '" placeholder="' + _placeholder + '" name="' + _name + '" readonly />');//value="' + moment(new Date()).format("YYYY-MM-DD") + '" 
                $('input[type="text"][name="' + _name + '"]').datetimepicker({
                    timepicker: false,
                    format: 'Y-m-d',
                    formatDate: 'Y-m-d',
                    minDate: new Date()
                });
                break;
            case "option":
                var htmlOption = '<select class='TableFieldFilter' + Type + ' TableFieldFilterOption" name="' + _name + '">';
                htmlOption += '<option value="" selected="selected">' + _placeholder + '</option>';

                var _urlOption = $(this).attr("url");
                $.ajax({
                    type: 'GET',
                    url: _urlOption,
                    dataType: 'json',
                    async: false,
                    //data: { get_param: 'value' },
                    success: function (dataOption) {
                        var _dataOption = dataOption["data"];
                        for (var key in _dataOption) {
                            var detailOption = _dataOption[key];
                            //console.log(detailOption['value']);
                            htmlOption += '<option value="' + detailOption['value'] + '">' + detailOption['name'] + '</option>';
                        }
                    }
                });

                htmlOption += '</select>';
                $(this).html(htmlOption);
                break;
            case "text":
                $(this).html('<input class='TableFieldFilter' + Type + ' TableFieldFilterText" type="text" format="' + _option + '" placeholder="' + _placeholder + '" name="' + _name + '" />');
                break;
            default:
                //$(this).html('');
                break;
        }
    });

    var table = $('#' + config.idTable).DataTable({
        destroy: true,
        //"dom": '<"top"i>rt<"top"flp><"clear">',
        "oLanguage": {
            "sDom": '<"top"i>rt<"top"flp><"clear">', // search colum
            "sLengthMenu": "Hiện thị _MENU_ bản ghi",
            "sZeroRecords": "Nothing found - sorry",
            "sInfo": "Hiện thị _START_ tới _END_ trong _TOTAL_ bản ghi",
            "sInfoEmpty": "Hiện thị 0 tới 0 trong 0 bản ghi",
            "sInfoFiltered": "(filtered from _MAX_ total records)",
            "oPaginate": {
                "sFirst": "Trang đầu",
                "sPrevious": "Trang trước",
                "sNext": "Trang sau",
                "sLast": "Trang cuối",
            }
        },
        "scrollX": scrollX,
        "fixedColumns": fixedColumns,
        "rowsGroup": rowsGroup,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": config.url,
            "type": "GET",
            "datatype": "json",
            "data": function (data) {
                delete data['columns'];
                delete data['search'];
                var objSearch = {};
                var objOrder = {};
                $('.TableFieldFilter' + Type).each(function (index) {
                    var _val = $(this).val();
                    var _name = $(this).attr("name");
                    var _format = $(this).attr("format");
                    objSearch[_name] = _val;
                });
                data["filter"] = JSON.stringify(objSearch);
                //console.log(data);               
                
                
            },
            //success(response) {
            //    var total = response.recordsTotal;

            //}
        },
        "iDisplayLength": config.length,
        "columns": config.column
    });

    // Apply the search
    table.columns().every(function () {
        var _this = this;
        var _old_date = '';

        //console.log(this);
        $('.TableFieldFilterText', this.header()).on('keypress', function (e) {
            if (e.keyCode == 13) {
                _this.search(this.value).draw();
            }
        });

        $('.TableFieldFilterOption', this.header()).change(function () {
            //if (this.value != _old_date) {
            _this.search(this.value).draw();
            //}
        });

        //$('.TableFieldFilterOption', this.header()).focusout(function () {
        //    //if (this.value != _old_date) {
        //        that.search(this.value).draw();
        //    //}
        //});
    });

    // selected row
    $('#' + config.idTable + ' tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    return table;
}

function refreshTable(tableId, urlData) {
    $.getJSON(urlData, null, function (json) {
        console.log(json);
        table = $(tableId).dataTable();
        oSettings = table.fnSettings();
        table.fnClearTable(this);
        for (var i = 0; i < json.data.length; i++) {
            table.oApi._fnAddData(oSettings, json.data[i]);
        }
        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        table.fnDraw();
    });
}