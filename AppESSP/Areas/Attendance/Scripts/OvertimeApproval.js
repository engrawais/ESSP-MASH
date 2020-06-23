function Create3() {
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
