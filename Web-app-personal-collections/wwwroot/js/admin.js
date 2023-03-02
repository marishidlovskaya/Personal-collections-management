$(document).ready(function () {
    $.ajax({
        url: "/Admin/GetUsers",
        type: "GET",
        dataType: "json",
        success: function (data) {
            InitUserTable(data);
        }
    });
    function InitUserTable(data) {
        $('#userstable').DataTable(
            {
                language: {
                    search: "Find user:"
                },
                "bLengthChange": false,
                info: false,
                data: data,
                "columns": [
                    { data: 'id' },
                    { data: 'email' },
                    { data: 'role' },
                    { data: 'status' },
                    {
                        "render": function (data, type, row, meta) {
                            return '<button type="button" class="edit-button-class btn btn-primary" style="width: 80px;" data-bs-toggle="modal" data-id="' + row.id + '" data-bs-target="#exampleModal">Edit</button> ' +
                                '<button type="button" class="delete-button-class btn btn-danger" style="width: 80px;" data-bs-toggle="modal" data-id="' + row.id + '" data-bs-target="#myModalDelete">Delete</button>' +
                                '<button type="button" class="ms-4 goToUserCollections btn" data-id="' + row.id + '">Collections</button>'
                        }
                    }
                ],
                columnDefs: [
                    {
                        target: 0,
                        visible: false,
                        searchable: true,
                    },
                ],
                "drawCallback": function (settings) {
                    $(".edit-button-class").on("click", function () {
                        var userId = $(this).data('id');
                        var user = GetUserData(userId);
                        $("#useremail").text(user.email);
                        $('#roles').val(user.role);
                        $('#statuses').val(user.status);
                        $('input[name=useridhash]').val(userId);
                    })
                    $(".delete-button-class").on("click", function () {
                        var userId = $(this).data('id');
                        $('input[name=useridfordeletion]').val(userId);
                    })
                    $(".goToUserCollections").on("click", function () {
                        document.location.href = '/ManageCollection/Index?id=' + $(this).data('id');
                    })
                }
            });
    }
    $("#confirmdeletion").on("click", function () {
        var id = $('#useridfordeletion').val();
        $.ajax({
            url: "/Admin/DeleteUserById",
            type: "POST",
            dataType: "json",
            data: { userId: id },
            success: function (data) {
                var rowIndexes = [];
                $('#userstable').DataTable().rows(function (idx, data, node) {
                    if (data.id === id) {
                        rowIndexes.push(idx);
                    }
                    return false;
                })
                $('#userstable').DataTable().row(rowIndexes[0]).remove().draw();
            }
        });
    })
    $("#saveuser").on("click", function () {
        var status = $('#statuses').val();
        var role = $('#roles').val();
        var id = $('#useridhash').val();
        $.ajax({
            url: "/Admin/UpdateUserData",
            type: "POST",
            dataType: "json",
            data: { userstatus: status, userrole: role, userid: id },
            success: function (data) {
                $('#exampleModal').modal('hide');
                var rowIndexes = [];
                $('#userstable').DataTable().rows(function (idx, data, node) {
                    if (data.id === id) {
                        rowIndexes.push(idx);
                    }
                    return false;
                });
                $('#userstable').DataTable().cell({ row: rowIndexes[0], column: 2 }).data(role);
                $('#userstable').DataTable().cell({ row: rowIndexes[0], column: 3 }).data(status);
            }
        });
    })
    function GetUserData(userId) {
        var result;
        $.ajax({
            url: "/Admin/GetUserData",
            type: "GET",
            dataType: "json",
            async: false,
            data: { userId: userId },
            success: function (data) {
                result = data;
            }
        });
        return result;
    }
    var myModal = document.getElementById('myModal')
    var myInput = document.getElementById('myInput')
    myModal.addEventListener('shown.bs.modal', function () {
        myInput.focus()
    })
})