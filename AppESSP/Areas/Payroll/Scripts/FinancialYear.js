// 
function FinancialYearGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Payroll/FinancialYear/Create",
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
function FinancialYearPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/Payroll/FinancialYear/Create",
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

function FinancialYearGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/Payroll/FinancialYear/Edit",
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
function FinancialYearPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/Payroll/FinancialYear/Edit",
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



function FinancialYearGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Payroll/FinancialYear/Delete",
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
function FinancialYearPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Payroll/FinancialYear/Delete",
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