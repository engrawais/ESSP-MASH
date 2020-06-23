function LeavePolicyGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/LeavePolicies/Create",
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
function LeavePolicyPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeavePolicies/Create",
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

function LeavePolicyGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/LeavePolicies/Edit",
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
function LeavePolicyPostEdit() {
    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeavePolicies/Edit",
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

function LeavePolicyGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/LeavePolicies/Delete",
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
function LeavePolicyPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeavePolicies/Delete",
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
function FormControlsScriptEdit(model) {

    if (model.WithFullPay == true) {
        $('input:radio[id=WithFullPay]').prop('checked', true);
    }
    if (model.WithHalfPay == true) {
        $('input:radio[id=WithHalfPay]').prop('checked', true);
    }
    if (model.WithOutPay == true) {
        $('input:radio[id=WithOutPay]').prop('checked', true);
    }

    //For Edit View Only
    if (model.ActiveAfterJoinDate == true) {
        $("#CustomDaysDataDiv").hide();
        $('input:radio[id=ActiveAfterJoinDate]').prop('checked', true);

    }
    if (model.ActiveAfterProbation == true) {
        $("#CustomDaysDataDiv").hide();
        $('input:radio[id=ActiveAfterProbation]').prop('checked', true);

    }
    if (model.ActiveAfterCustomDays == true) {
        $("#CustomDaysDataDiv").show();
        $('input:radio[id=ActiveAfterCustomDays]').prop('checked', true);
    }
}