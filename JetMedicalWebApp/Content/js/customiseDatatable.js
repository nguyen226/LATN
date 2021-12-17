/// <reference path="customiseDatatable.js" />
function initialDatatableOption(){
    return {
        "dom": "tlpir",
        "processing": true,
        "serverSide": true,
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 25,
        'aLengthMenu': [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'All']],
        'pagingType': 'numbers',
        "ajax": {
            "url": '',
            "type": "POST",
            "datatype":"json"
        },
        'order': [[1, "asc"]],
        //"scrollY": "50vh",
        "scrollX": "true",
        //"scrollCollapse": true,
        stateSave: false,
        'columnDefs':[],
        "columns": [],
        "language": {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Hiển thị _MENU_",
            "zeroRecords": "Không có dữ liệu",
            "info": "Trang _PAGE_ / _PAGES_",
            "infoEmpty": "Không có dữ liệu",
            "infoFiltered": "(Tìm thấy _MAX_ dòng)",
            "oAria": {
                "sSortAscending": "Nhấn vào để sắp xếp tăng dần",
                "sSortDescending": "Nhấn vào để sắp xếp giảm dần"
            },
            "sProcessing": "Đang xử lý...",
            "sLoadingRecords": "Đang tải dữ liệu...",
            "oPaginate": {
                "sFirst": "Đầu",
                "sLast": "Cuối",
                "sNext": "Sau",
                "sPrevious": "Trước"
            },
            "sEmptyTable": "Không có dữ liệu",
            "sInfo": "Hiển thị từ dòng _START_ đến _END_ trong tổng số _TOTAL_ dòng",
            "sSearch": "Tìm kiếm:"
        },
        "layout": {
            "theme": 'default', // datatable theme
            "class": '', // custom wrapper class
            "scroll": false, // enable/disable datatable scroll both horizontal and vertical when needed.
				// height: 450, // datatable's body's fixed height
            "footer": false // display/hide footer
			},
    };
}

//== Class definition

function initialDatatableLocalOption() {
    return {
        "filter": true, // this is for disable filter (search box)  
        "orderMulti": false, // for disable multiple column at once  
        "pageLength": 100,
        'aLengthMenu': [[10, 25, 50, 100, -1], [10, 25, 50, 100, 'Tất cả']],
        'pagingType': 'numbers',
        'dom': 'Brtlip',
        'order': [[1, "asc"]],
        "scrollY": "50vh",
        "scrollX": "true",
        "scrollCollapse": true,
        stateSave: false,
        'columnDefs': [],
        "columns": [],
        "language": {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Hiển thị _MENU_",
            "zeroRecords": "Không có dữ liệu",
            "info": "Trang _PAGE_ / _PAGES_",
            "infoEmpty": "Không có dữ liệu",
            "infoFiltered": "(Tìm thấy _MAX_ dòng)",
            "oAria": {
                "sSortAscending": "Nhấn vào để sắp xếp tăng dần",
                "sSortDescending": "Nhấn vào để sắp xếp giảm dần"
            },
            "sProcessing": "Đang xử lý...",
            "sLoadingRecords": "Đang tải dữ liệu...",
            "oPaginate": {
                "sFirst": "Đầu",
                "sLast": "Cuối",
                "sNext": "Sau",
                "sPrevious": "Trước"
            },
            "sEmptyTable": "Không có dữ liệu",
            "sInfo": "Hiển thị từ dòng _START_ đến _END_ trong tổng số _TOTAL_ dòng",
            "sSearch": "Tìm kiếm:"
        }
    };
}
