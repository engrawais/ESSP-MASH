
function EmployeeOtherGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/HumanResource/EmployeeOther/Create",
            contentType: "application/json; charset=utf-8",
            datatype: "json",
            success: function (url) {
                $('#modelBody').html(url);
                $('#myModal').modal('show');
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
}
function EmployeeOtherPostCreate() {
    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/EmployeeOther/Create",
            data: $("#formCreateID").serialize(),
            success: function (data) {
                if (data === "OK") {
                    $('#myModal').modal('hide');
                    location.reload();
                }
                else {
                    $('#modelBody').html(data);
                }
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
}
function EmployeeOtherGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/EmployeeOther/Edit",
        contentType: "application/json; charset=utf-8",
        data: { "id": id },
        datatype: "json",
        success: function (data) {
            $('#modelBody').html(data);
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function EmployeeOtherPostEdit() {
    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/EmployeeOther/Edit",
            data: $("#formEditID").serialize(),
            success: function (data) {
                if (data === "OK") {
                    $('#myModal').modal('hide');
                    location.reload();
                }
                else {
                    $('#modelBody').html(data);
                }
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
}
function EmployeeOtherGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/EmployeeOther/Delete",
        contentType: "application/json; charset=utf-8",
        data: { "id": id },
        datatype: "json",
        success: function (data) {
            $('#modelBody').html(data);
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function EmployeeOtherPostDelete() {
    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/EmployeeOther/Delete",
            data: $("#formDeleteID").serialize(),
            success: function (data) {
                if (data === "OK") {
                    $('#myModal').modal('hide');
                    location.reload();
                }
                else {
                    $('#modelBody').html(data);
                }
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
}

