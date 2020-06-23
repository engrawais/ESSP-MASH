function EmpLvAppGetCreate() {
    $('#btnGetCreate').click(function () {
        $.ajax({
            type: "GET",
            url: "/ESSP/ESSPLeaveApp/Create",
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
function LoadEmpInfo(EmpNo) {
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
    var finYearID = document.getElementById("FinancialYearID").value;
    var empNo = document.getElementById("EmpNo").value;
    $.ajax({
        type: "GET",
        url: "/Attendance/DD/GetEmpLeaveBalance",
        contentType: "application/json; charset=utf-8",
        data: { EmpNo: empNo, FinancialYearID: finYearID },
        datatype: "json",
        success: function (result) {
            $('#PVLB').html(result);

        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });

}
//function LoadfinInfo(EmpNo) {
//    var empNo = EmpNo;
//    var finYearID = document.getElementById("FinancialYearID").value;
//    $.ajax({
//        type: "GET",
//        url: "/Attendance/DD/GetfinLeaveBalance",
//        contentType: "application/json; charset=utf-8",
//        data: { EmpNo: empNo, FinancialYearID: finYearID },
//        datatype: "json",
//        success: function (result) {
//            $('#PVLB').html(result);

//        },
//        error: function () {
//            alert("Dynamic content load failed.");
//        }
//    });
//}
function EmpLvAppPostCreate() {

    $('#btnPostCreate').click(function () {

        var leaveTypeID = document.getElementById("LeaveTypeID").value;
        if (leaveTypeID == 3) {
            $.ajax({
                type: "POST",
                url: "/ESSP/ESSPLeaveApp/ValidateSLAttachment",
                data: $("#formCreateID").serialize(),
                success: function (data) {
                    if (data == "Required") {
                        var fileUpload = $("#CVUpload").get(0);
                        if (fileUpload.files.length == 0) {
                            // Display Message
                            alert("Attachment Required");
                        }
                        else {
                            EmpLvAppPostCreateHelper(leaveTypeID);
                        }
                    }
                    else {
                        EmpLvAppPostCreateHelper(leaveTypeID);
                    }
                },
                error: function () {
                    alert("Dynamic content load failed.");
                }
            });
        }
        else {
            EmpLvAppPostCreateHelper(leaveTypeID);
        }
    });
    document.getElementById("EName").innerHTML = "No Selected Employee";
    document.getElementById("EDesignation").innerHTML = "No Selected Employee";
    document.getElementById("EOU").innerHTML = "No Selected Employee";
    document.getElementById("EDOJ").innerHTML = "No Selected Employee";
    $("#IsHalfDiv").hide();
    $('#IsHalf').click(function () {
        if ($(this).is(":checked")) {
            $("#IsHalfDiv").show();
        }
        else {
            $("#IsHalfDiv").hide();
        }
    });
    $('#btnGetReplacementEmployee').click(function () {
        var empNo = document.getElementById("ReplacementEmpNo").value;
        $.ajax({
            url: '/Attendance/DD/GetReplacementEmployeeInfo',
            type: 'GET',
            cache: false,
            data: { ReplacementEmpNo: empNo },
            success: function (data) {
                document.getElementById("ERepName").innerHTML = data[0].ERepName;
                document.getElementById("ERepDesignation").innerHTML = data[0].ERepDesignation;
            },
            error: function () {
                $("#result").text('an error occured')

            }
        });
    });
    $("#NameLabel").hide();
    $("#PosLabel").hide();
    $('#btnGetReplacementEmployee').click(function () {
        $("#NameLabel").show();
        $("#PosLabel").show();
    });
    $('#DivLeaveCheck').show();
    $('#LeaveTypeID').change(function () {
        if ($("#LeaveTypeID").val() == "1") {
            $("#DivLeaveCheck").show();
        } else {
            $("#DivLeaveCheck").hide();
        }
    });
}
function EmpLvAppPostCreateHelper(leaveTypeID) {
    $.ajax({
        type: "POST",
        url: "/ESSP/ESSPLeaveApp/Create",
        data: $("#formCreateID").serialize(),
        success: function (data) {
            if (data === parseInt(data, 10)) {

                $('#myModal').modal('hide');
                if (leaveTypeID == 3) {
                    SaveCV(data);
                }
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
}
function EmpLvAppGetDelete(id) {
    $.ajax({
        type: "GET",
        url: "/ESSP/ESSPLeaveApp/Delete",
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
function EmpLvAppPostDelete() {

    $('#btnPostDelete').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPLeaveApp/Delete",
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
function CommentGetCreate(id) {
    $.ajax({
        type: "GET",
        url: "/ESSP/ESSPCommon/CommentView",
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        data: { "FormName": "Leave", "PID": id },
        success: function (url) {
            $('#modelBody').html(url);
            $('#myModal').modal('show');
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function CommentPostCreate() {
    $('#btnCommentPostCreate').click(function () {
        $.ajax({
            type: "POST",
            url: "/ESSP/ESSPCommon/CommentView",
            data: $("#formEditID").serialize(),
            success: function (data) {
                if (data === "OK") {
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
function SelectDeselectCheckBox() {
    var checkboxes = $(':checkbox');

    checkboxes.prop('checked', true);
    // Select/Deselect for Employee
    $("#chkSelectAllLvApp").bind("change", function () {
        $(".chkSelectLvApp").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectLvApp").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllLvApp").prop("checked", false);
    });
};
function SaveCV(id) {

    // Checking whether FormData is available in browser
    if (window.FormData !== undefined) {

        var fileUpload = $("#CVUpload").get(0);

        //Get name of CV
        var files = fileUpload.files;
        //Check extension of CV
    }
    // Create FormData object  
    var fileData = new FormData();
    var empNo = id;
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    // Adding one more key to FormData object  
    fileData.append("ID", id);
    $.ajax({
        url: '/ESSPLeaveApp/UploadFiles',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        success: function (result) {

            $('#myModal').modal('hide');
            location.reload();
        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}
function OpenCertificate(file) {
    var filename = file.toString();
    $.ajax({
        type: "GET",
        url: "/ESSP/ESSPLeaveApp/OpenCertificate",
        contentType: "application/json; charset=utf-8",
        data: { "fileName": filename },
        datatype: "json",
        success: function (data) {

        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function HideShowCertificate() {
    $('#LeaveTypeID').change(function () {
        HideControls();
    });
}
function HideControls() {
    if ($("#LeaveTypeID").val() == "3") {
        $("#CertificateDiv").show();
    } else {
        $("#CertificateDiv").hide();
    }
}