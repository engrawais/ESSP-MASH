
function ProcessRequestGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/Attendance/ProcessRequest/Create",
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
function ProcessRequestPostCreate() {
    if ($("#Criteria").val() == "E") {
        $("#emptb").show();
    } else {
        $("#emptb").hide();
    }
$('#btnPostCreate').click(function () {
    $.ajax({
        type: "POST",
        url: "/Attendance/ProcessRequest/Create",
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
$('#LocationDD').hide();
$('#Criteria').change(function () {
    if ($("#Criteria").val() == "L") {
        $("#LocationDD").show();
        $("#emptb").hide();
    } else {
        $("#LocationDD").hide();
    }
});
$('#Criteria').change(function () {
    if ($("#Criteria").val() == "E") {
        $("#emptb").show();
        $("#LocationDD").hide();
    } else {
        $("#emptb").hide();
    }
});
document.getElementById("EName").innerHTML = "No Selected Employee";
document.getElementById("EDesignation").innerHTML = "No Selected Employee";
document.getElementById("EOU").innerHTML = "No Selected Employee";
document.getElementById("EDOJ").innerHTML = "No Selected Employee";

$('#btnGetEmployee').click(function () {
    var empNo = document.getElementById("EmpNo").value;
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
});


}

function ProcessRequestGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/Attendance/ProcessRequest/Delete",
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


function ProcessRequestPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/Attendance/ProcessRequest/Delete",
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
function DDController() {
    $('#LocationDD').hide();
    $('#Criteria').change(function () {
        if ($("#Criteria").val() == "L") {
            $("#LocationDD").show();
        } else {
            $("#LocationDD").hide();
        }
    });
    $('#CatDD').show();
    $('#ProcessCat').change(function () {
        if ($("#ProcessCat").val() == "0") {
            $("#CatDD").hide();
        } else {
            $("#CatDD").show();
        }
    });
}
