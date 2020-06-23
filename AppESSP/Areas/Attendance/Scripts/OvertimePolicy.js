function OvertimePolicyGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/OvertimePolicy/Create",
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
function OvertimePolicyPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/OvertimePolicy/Create",
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

function OvertimePolicyGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/OvertimePolicy/Edit",
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
function OvertimePolicyPostEdit() {
    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/OvertimePolicy/Edit",
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

function OvertimePolicyGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/OvertimePolicy/Delete",
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
function OvertimePolicyPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/OvertimePolicy/Delete",
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

function FormControlsScript() 
{
    $("#PerdayOTHour").hide();
    $("#PerdayROTHour").hide();
    $("#PerdayGOTHour").hide();
    $("#EncashableOTHour").hide();

    $('#CalculateNOT').change(function () {
        if ($(this).is(":checked")) {
            $("#PerdayOTHour").show();
        }
        else {
            $("#PerdayOTHour").hide();
        }
    });
    $('#CalculateRestOT').change(function () {
        if ($(this).is(":checked")) {
            $("#PerdayROTHour").show();
        }
        else {
            $("#PerdayROTHour").hide();
        }
    });
    $('#CalculateGZOT').change(function () {
        if ($(this).is(":checked")) {
            $("#PerdayGOTHour").show();
        }
        else {
            $("#PerdayGOTHour").hide();
        }
    });
    $('#ConvertedToCPL').change(function () {
        if ($(this).is(":checked")) {
            $("#EncashableOTHour").show();
        }
        else {
            $("#EncashableOTHour").hide();
        }
    })

    //EDIT VIEW
    $(document).ready(function () {
        $("#EncashableOTHour").hide();
        $("#PerdayOTHour").hide();
        $("#PerdayROTHour").hide();
        $("#PerdayGOTHour").hide();
        
        if ($('#ConvertedToCPL').is(":checked")) {
            $("#EncashableOTHour").show();
        }
        if ($('#CalculateNOT').is(":checked")) {
            $("#PerdayOTHour").show();
        }
        if ($('#CalculateRestOT').is(":checked")) {
            $("#PerdayROTHour").show();
        }
        if ($('#CalculateGZOT').is(":checked")) {
            $("#PerdayGOTHour").show();
        }
        $('#ConvertedToCPL').change(function () {
            if ($(this).is(":checked")) {
                $("#EncashableOTHour").show();
            }
            else {
                $("#EncashableOTHour").hide();
            }
        })
        $('#CalculateNOT').change(function () {
            if ($(this).is(":checked")) {
                $("#PerdayOTHour").show();
            }
            else {
                $("#PerdayOTHour").hide();
            }
        })
        $('#CalculateRestOT').change(function () {
            if ($(this).is(":checked")) {
                $("#PerdayROTHour").show();
            }
            else {
                $("#PerdayROTHour").hide();
            }
        })
        $('#CalculateGZOT').change(function () {
            if ($(this).is(":checked")) {
                $("#PerdayGOTHour").show();
            }
            else {
                $("#PerdayGOTHour").hide();
            }
        })

    });
    
}