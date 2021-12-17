
function initDatatableMetronic (){
    //== Private functions
    return {
        // datasource definition
        data: {
            type: 'remote',
            source: {
                read: {
                    method: 'POST',
                    url: '',
                    map: function (raw) {
                        // sample data mapping
                        var dataSet = raw;
                        if (typeof raw.data !== 'undefined') {
                            dataSet = raw.data;
                        }
                        return dataSet;
                    },
                },
            },

            saveState: {
                cookie: false,
                webstorage: false,
            },

            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
        },

        // layout definition
        layout: {
            theme: 'default', // datatable theme
            class: '',
            scroll: false,
            footer: false,
            minHeight:400,
        },

        // column sorting
        sortable: true,

        pagination: true,

        toolbar: {
            // toolbar items
            items: {
                // pagination
                pagination: {
                    // page size select
                    pageSizeSelect: [10, 20, 30, 50, 100],
                },
            },
        },

        search: {
            input: $('#generalSearch'),
        },

        // columns definition
        columns: [],

        translate: {
            records: {
                processing: 'Đang tải dữ liệu...',
                noRecords: 'Không có dữ liệu',
            },
            toolbar: {
                pagination: {
                    items: {
                        default: {
                            first: 'Đầu',
                            prev: 'Trước',
                            next: 'Sau',
                            last: 'Cuối',
                            more: 'Xem thêm',
                            input: 'Trang số',
                            select: 'Chọn số lượng dòng',
                        },
                        info: 'Hiển thị từ dòng {{start}} - {{end}} trong tổng số {{total}} dòng',
                    },
                },
            },
        },
    };
};
