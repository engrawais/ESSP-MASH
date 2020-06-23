function JobTitleManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "JobTitle", "DivJobTitleItems", "LabelJobTitleCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "JobTitle", "DivJobTitleItems", "LabelJobTitleCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("JobTitle", "LabelJobTitleCount", "DivJobTitleItems", "TableJobTitle");
            }
            else// check
            {
                SelectAllValue("JobTitle", "DivJobTitleItems", "LabelJobTitleCount", "TableJobTitle");

            }
        }
    });


}


