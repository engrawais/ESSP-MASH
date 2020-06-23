function CheckBoxSelectionCreate3() {
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
function ForMaximumStringValidation(textBoxID, text_max, textAreaID) {
    $(textBoxID).html(text_max + ' Characters remaining.');

    $(textAreaID).keyup(function () {
        var text_length = $(textAreaID).val().length;
        var text_remaining = text_max - text_length;

        $(textBoxID).html(text_remaining + ' character remaining.');
    });
}
