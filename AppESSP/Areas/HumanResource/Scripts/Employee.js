function EmployeeGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/Employee/Edit",
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
function EmployeePostEdit() {

    $('#btnPostEdit').click(function () {
        var isValid = true;
        if ($('#ExistingCrewID').val() != $("#CrewID").val()) {
            if ($('#CrewStartDate').val() == "")
                isValid = false;
            if ($('#CrewEndDate').val() == "")
                isValid = false;
        }
        if (isValid == true) {
            $.ajax({
                type: "POST",
                url: "/HumanResource/Employee/Edit",
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

            var formData = new FormData();
            var empid = document.getElementById("PEmployeeID").value;
            var totalFiles = document.getElementById("imgupload").files.length;
            for (var i = 0; i < totalFiles; i++) {
                var _file = document.getElementById("imgupload").files[i];
                formData.append("imgupload", _file);
            }
            formData.append("EmpID", empid);
            $("#divLoading").show();
            // for Image
            $.ajax({
                url: '/HumanResource/Employee/EPImage',
                type: 'POST',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function () {
                },
                error: function () {
                    $("#result").text('an error occured')
                }
            });
        }
        else {alert("Start Date and End date cannot be empty")}

    });

}

function EmployeeGetCreate() {
       $.ajax({
            type: "GET",
            url: "/HumanResource/Employee/Create",
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
function EmployeePostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/Employee/Create",
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
