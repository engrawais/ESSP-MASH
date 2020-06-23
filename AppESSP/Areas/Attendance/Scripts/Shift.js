// 
function ShiftGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/Shift/Create",
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
function ShiftPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/Shift/Create",
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

function ShiftGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/Shift/Edit",
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
function ShiftPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/Shift/Edit",
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



function ShiftGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/Shift/Delete",
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
function ShiftPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/Shift/Delete",
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