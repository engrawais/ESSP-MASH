// 
function ShiftCHEmpGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/EmployeeShiftChange/Create",
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
function ShiftCHEmpPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/EmployeeShiftChange/Create",
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
    document.getElementById("EName").innerHTML = "No Selected Employee";
    document.getElementById("EDesignation").innerHTML = "No Selected Employee";
    document.getElementById("EOU").innerHTML = "No Selected Employee";
    document.getElementById("EDOJ").innerHTML = "No Selected Employee";

    $('#btnGetEmployee').click(function () {
        var empNo = document.getElementById("EmpNo").value;
        $.ajax({
            url: '/Attendance/DD/GetEmployeeInfo',
            type: 'GET',
            cache: false,
            data: { EmpNo: empNo },
            success: function (data) {
                document.getElementById("EName").innerHTML = data[0].EName;
                document.getElementById("EDesignation").innerHTML = data[0].EDesignation;
                document.getElementById("EOU").innerHTML = data[0].EOU;
                document.getElementById("EDOJ").innerHTML = data[0].EDOJ;

            },
            error: function () {
                $("#result").text('an error occured')

            }
        });
    });
}
function ShiftCHEmpGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/EmployeeShiftChange/Edit",
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
function ShiftCHEmpPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/EmployeeShiftChange/Edit",
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



function ShiftCHEmpGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/EmployeeShiftChange/Delete",
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
function ShiftCHEmpPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/EmployeeShiftChange/Delete",
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