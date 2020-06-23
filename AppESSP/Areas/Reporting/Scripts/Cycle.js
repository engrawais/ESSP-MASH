function CycleManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Cycle", "DivCycleItems", "LabelCycleCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Cycle", "DivCycleItems", "LabelCycleCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Cycle", "LabelCycleCount", "DivCycleItems", "TableCycle");
            }
            else// check
            {
                SelectAllValue("Cycle", "DivCycleItems", "LabelCycleCount", "TableCycle");

            }
        }
    });


}


