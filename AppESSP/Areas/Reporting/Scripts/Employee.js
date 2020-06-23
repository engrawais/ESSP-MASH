function EmployeeManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Employee", "DivEmployeeItems", "LabelEmployeeCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Employee", "DivEmployeeItems", "LabelEmployeeCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Employee", "LabelEmployeeCount", "DivEmployeeItems", "TableEmployee");
            }
            else// check
            {
                SelectAllValue("Employee", "DivEmployeeItems", "LabelEmployeeCount", "TableEmployee");

            }
        }
    });


}


