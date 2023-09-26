var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/Processor/GetAll"
        },
        "columns": [
            { "data": "name", "width": "70%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text center">
<a href="/Admin/Processor/Upsert/${data}"class="btn btn-info")><i class="fas fa-edit"></i></a>
<a class="btn btn-danger" onclick=Delete("/Admin/Processor/Delete/${data}")><i class="fas fa-trash-alt"></i></a>
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
                        // toastr.success(data.message);
                        swal({ icon: "success", text: "Data Delete succesfully", timer: 1200 });

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