function LoadJTCommon(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/JTCommon/Edit",
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

function JTCommonGetEdit(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/JTCommon/Edit",
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
function JTCommonPostEdit() {

    $('#btnPostEdit').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/JTCommon/Edit",
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

function JTCommonGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/HumanResource/JTCommon/Create",
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
function JTCommonPostCreate() {

    $('#btnPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/JTCommon/Create",
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

function JTCommonGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/JTCommon/Delete",
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


function JTCommonPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/JTCommon/Delete",
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

