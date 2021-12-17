function getListTaiKhoanAutocompleteAjaxOption(url, selectedFields, condition, sortCondition, response) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: 'json',
        data: {
            selectedFields: selectedFields,
            stringFilter: condition,
            sortCondition: sortCondition,
            top: 10
        },
        success: function (data) {
            data.push({
                Ten: "Không chọn",
                Ma: "",
                Id: -1
            });


            response($.map(data, function (item) {
                var data = {
                    label: item.Ten,
                    Ma: item.Ma,
                    value: item.Id,
                };
                return data;
            }));
        }
    });
}



//$('.clickToEdit').autocomplete({
//    source: function (request, response) {
//        var selectedFields = "Id, Ma, Ten, ApDungChoTatCaDonVi, (ThongTu != null ? ThongTu.Id : -1) AS ThongTuId, TaiKhoan_DonVis";
//        var condition = 'ThongTuId = ' + thongTu.id + ' AND (ApDungChoTatCaDonVi == TRUE OR TaiKhoan_DonVis.Any(DonVi.Id = ' + $(elementId_ThaoTacCaNhan_DonViIdLamViecHienTai).val() + '))';
//        condition += request.term ? ' AND (Ma.Contains(\\' + request.term + '\\) OR Ten.Contains(\\' + request.term + '\\))' : '';

//        getListTaiKhoanAutocompleteAjaxOption('@Url.Action("Filter", "TaiKhoan", new { area = "Admin" })', selectedFields, condition, 'Ma asc', response)
//    },
//    minLength: 1,
//    open: function () {
//        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
//    },
//    close: function () {
//        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
//    },
//    change: function (event, ui) {
//        if (dataRowSelected.TaiKhoanCoId < -1 || $(this).text() != dataRowSelected.TenTaiKhoanCo) {
//            tableChiTiet.row(rowIndex).data(dataRowSelected).invalidate().draw();
//        }
//        $('#dtTableChiTiet tbody tr:eq(' + rowIndex + ') td:eq(' + colIndex + ')').find('.row_data').addClass('clickToEdit').focus();
//    },
//    select: function (event, ui) {
//        if (!ui.item) {
//            tableChiTiet.row(rowIndex).data(dataRowSelected).invalidate().draw();
//        } else {
//            dataRowSelected.TaiKhoanCoId = ui.item.value;
//            dataRowSelected.MaTaiKhoanCo = ui.item.Ma;
//            dataRowSelected.TenTaiKhoanCo = ui.item.label;
//            tableChiTiet.row(rowIndex).data(dataRowSelected).invalidate().draw();
//        }
//    }
//});