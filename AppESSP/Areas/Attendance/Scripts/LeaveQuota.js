function DDFYChnage() {
    document.getElementById('FinancialYearID').onchange = function () {
        var finYearID = document.getElementById('FinancialYearID').value;
        window.location = "/Attendance/LeaveQuota/Index?FinancialYearID=" + finYearID;
    };
}
function CPLGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/LeaveQuota/CPLDetailEdit",
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
function CPLPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeaveQuota/CPLDetailEdit",
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
function CPLGetCreate(id) {
        $.ajax({
            type: "GET",
            url: "/Attendance/LeaveQuota/CPLCreate",
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
function CPLPostCreate() {
        $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/LeaveQuota/CPLCreate",
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