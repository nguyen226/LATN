function initialDropdownlistSelect2Option(){
    return {
        language: {
            noResults: function (term) {
                return "Không tìm thấy.";
            },
            searching: function (term) {
                return "Đang tìm...";
            }
        }
    };
}
