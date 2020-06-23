function ShiftManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID,"Shift", "DivShiftItems", "LabelShiftCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Shift", "DivShiftItems", "LabelShiftCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Shift", "LabelShiftCount", "DivShiftItems", "TableShift");
            }
            else// check
            {
                SelectAllValue("Shift", "DivShiftItems", "LabelShiftCount","TableShift");
                
            }
        }
    });


}


