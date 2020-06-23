function StepOne() {
    LoadRadioButtonTables("rbAll");
    $("#JCValueDiv").hide();
    var value = $("input[name$='SelectionRB']:checked").val();
    LoadRadioButtonTables(value);
    $("input[name$='SelectionRB']").click(function () {
        var value = $(this).val();
        LoadRadioButtonTables(value);
    });

    // Select/Deselect for Company
    $("#chkSelectAllDivCompany").bind("change", function () {
        $(".chkSelectDivCompany").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectDivCompany").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllDivCompany").prop("checked", false);
    });
    // Select/Deselect for OU Common
    $("#chkSelectAllOUCommon").bind("change", function () {
        $(".chkSelectOUCommon").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectOUCommon").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllOUCommon").prop("checked", false);
    });
    // Select/Deselect for OU
    $("#chkSelectAllOU").bind("change", function () {
        $(".chkSelectOU").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectOU").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllOU").prop("checked", false);
    });
    // Select/Deselect for EmploymentType
    $("#chkSelectAllEmploymentType").bind("change", function () {
        $(".chkSelectEmploymentType").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectEmploymentType").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllEmploymentType").prop("checked", false);
    });
    // Select/Deselect for Location
    $("#chkSelectAllLocation").bind("change", function () {
        $(".chkSelectLocation").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectLocation").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllLocation").prop("checked", false);
    });
    // Select/Deselect for Grade
    $("#chkSelectAllGrade").bind("change", function () {
        $(".chkSelectGrade").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectGrade").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllGrade").prop("checked", false);
    });
    // Select/Deselect for JobTitle
    $("#chkSelectAllJobTitle").bind("change", function () {
        $(".chkSelectJobTitle").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectJobTitle").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllJobTitle").prop("checked", false);
    });

    // Select/Deselect for Designation
    $("#chkSelectAllDesignation").bind("change", function () {
        $(".chkSelectDesignation").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectDesignation").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllDesignation").prop("checked", false);
    });
    // Select/Deselect for Crew
    $("#chkSelectAllCrew").bind("change", function () {
        $(".chkSelectCrew").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectCrew").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllCrew").prop("checked", false);
    });
    // Select/Deselect for Shift
    $("#chkSelectAllShift").bind("change", function () {
        $(".chkSelectShift").prop("checked", $(this).prop("checked"));
    });
    $(".chkSelectShift").bind("change", function () {
        if (!$(this).prop("checked"))
            $("#chkSelectAllShift").prop("checked", false);
    });
};
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
function LoadRadioButtonTables(value) {
    if (value == "rbAll") {
        $("#Divheader2").hide();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbCompany") {
        $("#Divheader2").show();
        $("#DivCompany").show();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbOUCommon") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").show();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbOU") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").show();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbEmploymentType") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").show();
        $("#DivLocation").hide();
        $("#DivGrade").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbLocation") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivLocation").show();
        $("#DivGrade").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbGrade") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").show();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbJobTitle") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").show();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbDesignation") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").show();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }
    if (value == "rbShift") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").hide();
        $("#DivShift").show();
    }

    if (value == "rbCrew") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivGrade").hide();
        $("#DivLocation").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").show();
        $("#DivEmployee").hide();
        $("#DivShift").hide();
    }

    if (value == "rbEmployee") {
        $("#Divheader2").show();
        $("#DivCompany").hide();
        $("#DivOUCommon").hide();
        $("#DivOU").hide();
        $("#DivEmploymentType").hide();
        $("#DivLocation").hide();
        $("#DivGrade").hide();
        $("#DivJobTitle").hide();
        $("#DivDesignation").hide();
        $("#DivCrew").hide();
        $("#DivEmployee").show();
        $("#DivShift").hide();
    }
}