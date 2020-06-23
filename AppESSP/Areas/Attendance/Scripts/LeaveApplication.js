function LvAppGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/LeaveApplication/Create",
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
function LvFromDateToDate() {
    $('#ToDate').change(function () {
        if ($('#FromDate').val() == '') { alert("Kindly Select From Date"); }
        else {
            if (this.value == $('#FromDate').val()) { $('#DivIsHalf').show(); }
            else { $('#DivIsHalf').hide(); }
        }
    });
}
function LvAppPostCreate() {
    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeaveApplication/Create",
            data: $("#formCreateID").serialize(),
            success: function (data) {
                if (data === "OK") {
                    $.ajax({
                        type: "GET",
                        url: "/Attendance/LeaveApplication/Create",
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
function GetEmployee() {
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
            data: { EmpNo: empNo},
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
        var finYearID = document.getElementById("FinancialYearID").value;
        $.ajax({
            type: "GET",
            url: "/Attendance/DD/GetEmpLeaveBalance",
            contentType: "application/json; charset=utf-8",
            data: { EmpNo: empNo, FinancialYearID:finYearID  },
            datatype: "json",
            success: function (result) {
                $('#PVLB').html(result);

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
    $('#DivLeaveCheck').show();
    $('#LeaveTypeID').change(function () {
        if ($("#LeaveTypeID").val() == "1") {
            $("#DivLeaveCheck").show();
        } else {
            $("#DivLeaveCheck").hide();
        }
    });
}
function ReplacementEmployee() {

    $("#IsHalfDiv").hide();
    $('#IsHalf').click(function () {
        if ($(this).is(":checked")) {
            $("#IsHalfDiv").show();
        }
        else {
            $("#IsHalfDiv").hide();
        }
    });
}
function LvAppGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/LeaveApplication/Edit",
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
function LvAppPostEdit() {
    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeaveApplication/Edit",
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

    if (document.getElementById('IsHalf').checked) {
        $("#IsHalfDiv").show();
    }
    else {
        $("#IsHalfDiv").hide();
    }
}



function LvAppGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/LeaveApplication/Delete",
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
function LvAppPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeaveApplication/Delete",
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