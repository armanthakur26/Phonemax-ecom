var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //userlocked
                        return `
                        <div class="text-center">
                        <a class="btn btn-danger" onclick=lockunlock("${data.id}")>Unlock</a>
                        </div>
                            `;
                    }
                    else {

                        return `
                            <div class="text-center">
                                <a class="btn btn-success" onclick=lockunlock("${data.id}") > lock</a >
                        </div>
                            `;
                    }
                }
            }

        ]
    })
}

function lockunlock(id) {
    // alert(id);
    $.ajax({
        url: "/admin/User/lockunlock",
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                // toastr.success(data.message);
                swal({ title: "User locked", icon: "success", buttons: false, timer: 1200 })
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}
