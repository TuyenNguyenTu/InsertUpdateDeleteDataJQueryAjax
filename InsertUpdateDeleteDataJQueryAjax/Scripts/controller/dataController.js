var dataConfig = {
    pageSize: 5,
    pageIndex: 1
}
var dataController = {
    init: function () {
        dataController.LoadData();
        dataController.registerEvent();

    },
    registerEvent: function () {
        $('.txtName').off('keypress').on('keypress', function (e) {
            if (e.which == 13) {
                var id = $(this).data('id');
                var value = $(this).val();
                dataController.updateData(id, value);
            }
        })
    },
    updateData: function (id, value) {
        var data = {
            ID: id,
            Name: value
        };
        $.ajax({
            url: 'Update',
            type: 'POST',
            dataType: 'Json',
            // truyen len = 1 bien: chuyen chuoi sang dang string
            data: { model: JSON.stringify(data) },
            success: function (response) {
                if (response.status) {
                    alert("Update success.");
                } else {
                    alert("Update failed.");
                }
            }
        })
    },
    LoadData: function () {
        $.ajax({
            url: 'GetJsonData',
            type: 'GET',
            data: {
                page: dataConfig.pageIndex,
                pageSize: dataConfig.pageSize
            },
            dataType: 'JSON',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.ID,
                            Name: item.Name,
                            Address: item.Address,
                            Status: item.Status == true ? "<button type=\"button\" class='btn btn-success'>Active</button>" : "<button type=\"button\" class='btn btn-danger'>Danger</button>"
                            
                        });
                    });
                    $('#tbl_data').html(html);
                    dataController.paging(response.total, function () {
                        dataController.LoadData();
                    });
                    dataController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback) {
        var totalPage = Math.ceil(totalRow / dataConfig.pageSize)
        $('#pagination').twbsPagination({
            totalPages: totalPage,
            visiblePages: 10,
            onPageClick: function (event, page) {
                dataConfig.pageIndex = page;
                //set xong sau 0,2s sau goi ham 
                setTimeout(callback, 200);
            }
        });
    }

}
dataController.init();