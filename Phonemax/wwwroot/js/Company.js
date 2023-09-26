var dataTable;
$(document).ready(function () {
    loaddatatable();
})
function loaddatatable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "isAuthorizedCompany", "width": "10%",
                "render": function (data) {
                    if (data) return `
                    <input type="checkbox"checked disabled/>`;
                    else {
                        return `<input type="checkbox" disabled />`;
                    }
                }
            },

            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text center">
<a href="/admin/company/Upsert/${data}"class="btn btn-info")><i class="fas fa-edit"></i></a>
<a class="btn btn-danger" onclick=Delete("/admin/company/Delete/${data}")><i class="fas fa-trash-alt"></i></a>
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