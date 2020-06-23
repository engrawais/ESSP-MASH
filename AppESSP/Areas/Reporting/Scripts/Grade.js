function GradeManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Grade", "DivGradeItems", "LabelGradeCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Grade", "DivGradeItems", "LabelGradeCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Grade", "LabelGradeCount", "DivGradeItems", "TableGrade");
            }
            else// check
            {
                SelectAllValue("Grade", "DivGradeItems", "LabelGradeCount", "TableGrade");

            }
        }
    });


}


