// 
function ShiftCHGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/ShiftChange/Create",
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
function ShiftCHPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/ShiftChange/Create",
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
    $("#hasbreakdiv").hide();
    $('#HasBreak').change(function () {
        if ($(this).is(":checked")) {
            $("#hasbreakdiv").show();
        }
        else {
            $("#hasbreakdiv").hide();
        }
    });

}
function ShiftCHGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/ShiftChange/Edit",
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
function ShiftCHPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/ShiftChange/Edit",
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
    if (document.getElementById('HasBreak').checked) {
        $("#DivHasBreak").show();
    }
    else {
        $("#DivHasBreak").hide();
    }
    $('#HasBreak').change(function () {
        if ($(this).is(":checked")) {
            $("#DivHasBreak").show();
        }
        else {
            $("#DivHasBreak").hide();
        }


    });
}

function ShiftCHGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/ShiftChange/Delete",
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
function ShiftCHPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/ShiftChange/Delete",
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