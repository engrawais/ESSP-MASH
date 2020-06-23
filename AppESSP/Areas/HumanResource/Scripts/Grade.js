function LoadDesig(id) {
    $.ajax({
        type: "GET",
        url: "/HumanResource/Grade/Edit",
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
function IndexJS() {

    $('#btnSave').click(function () {
        $.ajax({
            type: "POST",
            url: "/HumanResource/Grade/Edit",
            data: $("#frmGrade").serialize(),
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