function LoadEmployeeInformation() {
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
function StepTwo() {
    var checkboxes = $(':checkbox');

    checkboxes.prop('checked', true);
    // Select/Deselect for Employee
    $("#chkSelectAllEmp").bind("change", function () {
        $(".chkSelectEmp").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectEmp").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllEmp").prop("checked", false);
    });
};