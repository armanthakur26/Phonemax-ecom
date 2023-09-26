var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "10%" },
            { "data": "discription", "width": "30%" },
            { "data": "ram", "width": "10%" },
            { "data": "rom", "width": "10%" },
            { "data": "battery", "width": "10%" },
            { "data": "price", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="text-center">
                             <a href="/Admin/Product/Upsert/${data}" class="btn btn-info">
                             <i class="fas fa-edit"></i>
                             </a>
                             <a class="btn btn-danger" onClick=Delete("/Admin/Product/Delete/${data}")>
                             <i class="fas fa-trash-alt"></i>
                             </a>
                             </div>
                           `;
                }
            }
        ]
    })
}
function Delete(url) {
    //alert(url);
    swal({
        title: "Want To Delete Data?",
        text: "Info Will be Deleted!!!",
        buttons: true,
        icon: "warning",
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })

        }
    })
}