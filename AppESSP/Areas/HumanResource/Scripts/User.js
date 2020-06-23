function UserCBAndTableControls() {
    //RECRUITMENT
    $('#RMS').change(function () {

        if ($(this).is(":checked")) {
            $("#RecruitmentDiv").show();
        }
        else {
            $("#RecruitmentDiv").hide();
        }
    });
    if (document.getElementById('RMS').checked) {
        $("#RecruitmentDiv").show();
    } else {
        $("#RecruitmentDiv").hide();
    }
    //PERFORMANCE
    $('#PMS').change(function () {

        if ($(this).is(":checked")) {
            $("#PerformanceDiv").show();
        }
        else {
            $("#PerformanceDiv").hide();
        }
    });
    if (document.getElementById('PMS').checked) {
        $("#PerformanceDiv").show();
    } else {
        $("#PerformanceDiv").hide();
    }
    //MCOMPANY
    $('#MCompany').change(function () {

        if ($(this).is(":checked")) {
            $("#MCompanyDiv").show();
        }
        else {
            $("#MCompanyDiv").hide();
        }
    });
    if (document.getElementById('MCompany').checked) {
        $("#MCompanyDiv").show();
    } else {
        $("#MCompanyDiv").hide();
    }
    //MATTENDANCEDIV
    $('#MAttendance').change(function () {

        if ($(this).is(":checked")) {
            $("#MAttDiv").show();
        }
        else {
            $("#MAttDiv").hide();
        }
    });
    if (document.getElementById('MAttendance').checked) {
        $("#MAttDiv").show();
    } else {
        $("#MAttDiv").hide();
    }
    // Leaves
    $('#MLeave').change(function () {
        if ($(this).is(":checked")) {
            $("#LeaveDiv").show();
        }
        else {
            $("#LeaveDiv").hide();
        }
    });
    if (document.getElementById('MLeave').checked) {
        $("#LeaveDiv").show();
    } else {
        $("#LeaveDiv").hide();
    }
    // Shift
    $('#MShift').change(function () {
        if ($(this).is(":checked")) {
            $("#ShiftDiv").show();
        }
        else {
            $("#ShiftDiv").hide();
        }
    });
    if (document.getElementById('MShift').checked) {
        $("#ShiftDiv").show();
    } else {
        $("#ShiftDiv").hide();
    }
    // Overtime
    $('#MOvertime').change(function () {
        if ($(this).is(":checked")) {
            $("#OvertimeDiv").show();
        }
        else {
            $("#OvertimeDiv").hide();
        }
    });
    if (document.getElementById('MOvertime').checked) {
        $("#OvertimeDiv").show();
    } else {
        $("#OvertimeDiv").hide();
    }
    // Attendance Editor
    $('#MAttendanceEditor').change(function () {
        if ($(this).is(":checked")) {
            $("#AttendanceEditorDiv").show();
        }
        else {
            $("#AttendanceEditorDiv").hide();
        }
    });
    if (document.getElementById('MAttendanceEditor').checked) {
        $("#AttendanceEditorDiv").show();
    } else {
        $("#AttendanceEditorDiv").hide();
    }
    // Settings
    $('#MSettings').change(function () {
        if ($(this).is(":checked")) {
            $("#SettingsDiv").show();
        }
        else {
            $("#SettingsDiv").hide();
        }
    });
    if (document.getElementById('MSettings').checked) {
        $("#SettingsDiv").show();
    } else {
        $("#SettingsDiv").hide();
    }
    // User
    $('#MUser').change(function () {
        if ($(this).is(":checked")) {
            $("#UserDiv").show();
        }
        else {
            $("#UserDiv").hide();
        }
    });
    if (document.getElementById('MUser').checked) {
        $("#UserDiv").show();
    } else {
        $("#UserDiv").hide();
    }
    $("#LocationTableDiv").hide();
    $("#DepartmentTableDiv").hide();
 if ($("#UserAccessTypeID").val() == "2") {
            $("#LocationTableDiv").show();
        } else {
            $("#LocationTableDiv").hide();
    }
    if ($("#UserAccessTypeID").val() == "4") {
        $("#DepartmentTableDiv").show();
    } else {
        $("#DepartmentTableDiv").hide();
    }
    $("#UserAccessTypeID").change(function () {

        if ($(this).val() == "2") {
            $("#LocationTableDiv").show();
        } else {
            $("#LocationTableDiv").hide();
        }
        if ($(this).val() == "4") {
            $("#DepartmentTableDiv").show();
        } else {
            $("#DepartmentTableDiv").hide();
        }

    });
    
}

function UserGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/HumanResource/User/Create",
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
function UserPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/User/Create",
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
