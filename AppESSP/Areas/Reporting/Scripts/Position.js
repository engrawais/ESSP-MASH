function PositionManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Position", "DivPositionItems", "LabelPositionCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Position", "DivPositionItems", "LabelPositionCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Position", "LabelPositionCount", "DivPositionItems", "TablePosition");
            }
            else// check
            {
                SelectAllValue("Position", "DivPositionItems", "LabelPositionCount", "TablePosition");

            }
        }
    });


}


