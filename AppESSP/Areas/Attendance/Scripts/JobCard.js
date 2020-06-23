function Create1()
{
    $("#JCValueDiv").hide();
    $("#JobCardTypeID").change(function () {
        var val = this.value;
        if (val == 9 || val == 10) {
            $("#JCValueDiv").show();
        }
    });
}
function IndexDetail1()
{
   
}
function Create2() {
    var checkboxes = $(':checkbox');

    checkboxes.prop('checked', true);
    // Select/Deselect for Employee
    $("#chkSelectAllEmp").bind("change", function () {
        $(".chkSelectEmp").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectEmp").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllEmp").prop("checked", false);
    });
}

function SingleDayGetCreate() {
    $('#btnGetSingleDayCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/JobCard/SingleDay",
         
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
function SingleDayPostCreate() {

    $('#btnPostSingleDayCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/JobCard/SingleDay",
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
function MultipleDayGetCreate() {
    $('#btnGetMultipleDayCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/JobCard/MultipleDay",
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
        $('#EmpNo').focus()
    })
}

function MultipleDayPostCreate() {
    $('#btnPostMultipleDayCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/JobCard/MultipleDay",
            data: $("#formCreateID").serialize(),
         success: function (data) {
                if (data === "OK") {
                    $.ajax({
                        type: "GET",
                       url: "/Attendance/JobCard/MultipleDay",
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
function EmployeeInfo ()
    {
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
function JobCardGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/JobCard/Delete",
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
function JobCardPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/JobCard/Delete",
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

