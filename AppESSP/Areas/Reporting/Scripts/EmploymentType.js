function EmployementTypeManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "EmployementType", "DivEmployementTypeItems", "LabelEmployementTypeCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "EmployementType", "DivEmployementTypeItems", "LabelEmployementTypeCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("EmployementType", "LabelEmployementTypeCount", "DivEmployementTypeItems", "TableEmployementType");
            }
            else// check
            {
                SelectAllValue("EmployementType", "DivEmployementTypeItems", "LabelEmployementTypeCount", "TableEmployementType");

            }
        }
    });


}


