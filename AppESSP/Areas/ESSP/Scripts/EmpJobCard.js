function EmpJobCardGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/ESSP/ESSPJobCard/Create",
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
function SingleDayGetCreate() {
    $('#btnGetSingleDayCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/ESSP/ESSPJobCard/SingleDay",
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
function SingleDayPostCreate() {

    $('#btnPostSingleDayCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPJobCard/SingleDay",
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
function EmpJobCardPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPJobCard/Create",
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
function LoadEmpInfo(EmpNo) {
    var empNo = EmpNo;
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
}
function EmpJobCardGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/ESSP/ESSPJobCard/Delete",
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
function EmpJobCardPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPJobCard/Delete",
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
function CommentGetCreate(id) {
    $.ajax({
        type: "GET",
        url: "/ESSP/ESSPCommon/CommentView",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        data: { "FormName": "JobCard", "PID": id },
        success: function (url) {
            $('#modelBody').html(url);
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function CommentPostCreate() {
    $('#btnCommentPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPCommon/CommentView",
            data: $("#formEditID").serialize(),
            success: function (data) {
                if (data === "OK") {
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
function SelectDeselectCheckBox() {
    var checkboxes = $(':checkbox');

    checkboxes.prop('checked', true);
    // Select/Deselect for Employee
    $("#chkSelectAllJcApp").bind("change", function () {
        $(".chkSelectJcApp").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectJcApp").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllJcApp").prop("checked", false);
    });
};
