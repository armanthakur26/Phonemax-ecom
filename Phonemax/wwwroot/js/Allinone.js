var dataTable;
$(document).ready(function () {
    loadDataTable();

})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Allinone/GetAll"
        },
        "columns": [
            { "data": "name", "width": "18%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "paymentStatus", "width": "18%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "orderTotal", "width": "18%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            { "data": "orderStatus", "width": "18%", "render": function (data) { return `<p class="text-center">${data}<p>`; } },
            {
                "data": "orderDate", "width": "18%", "render": function (data) { return `<p class="text-center">${data}<p>`; },
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : "0" + month) + "/" + date.getDate() + "/" + date.getFullYear() + "&nbsp;&nbsp;" + (date.getHours() < 10 ? ("0" + date.getHours()) : date.getHours()) + ":" + (date.getMinutes() < 10 ? ("0" + date.getMinutes()) : date.getMinutes());
                    //return date;
                }
            },
            {
                "data": "id", "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/Admin/Allinone/Upsert/${data}"class="btn btn-success">
                    <i class="fas fa-info-circle"></i>
                    </a>
                    </div>
                    `;
                }
            }
        ]
    })
}





