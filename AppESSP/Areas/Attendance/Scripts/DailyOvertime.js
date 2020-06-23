
function DailyOvertimeGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/DailyOvertime/Create",
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
    $('#myModal').on('shown.bs.modal', function () {
        $('#EmpNo').focus();
    });
}

function DailyOvertimePostCreate() {
    $('#btnPostCreate').click(function () {
        if (confirm('Are you sure you want to save this Overtime?')) {
            $.ajax({
                type: "POST",
                url: "/Attendance/DailyOvertime/Create",
                data: $("#formCreateID").serialize(),
                success: function (data) {
                    if (data === "OK") {
                        $.ajax({
                            type: "GET",
                            url: "/Attendance/DailyOvertime/Create",
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
        } else {
            location.reload();
        }
      
    });

}
function EmployeeInfo() {
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

function DailyOvertimeGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/DailyOvertime/Delete",
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


function DailyOvertimePostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/DailyOvertime/Delete",
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