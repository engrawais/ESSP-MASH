function LoadUserRole(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/UserRole/Edit",
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

function UserRoleGetEdit(id) {
    alert(id);
    $.ajax({
        type: "GET",
        url: "/HumanResource/UserRole/Edit",
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
function UserRolePostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/UserRole/Edit",
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

function UserRoleGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/HumanResource/UserRole/Create",
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
function UserRolePostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/UserRole/Create",
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
function UserRoleGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/UserRole/Delete",
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


function UserRolePostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/UserRole/Delete",
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
function UserRoleCBControls() {
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

}