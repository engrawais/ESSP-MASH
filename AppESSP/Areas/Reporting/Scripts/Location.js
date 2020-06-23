function LocationManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Location", "DivLocationItems", "LabelLocationCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Location", "DivLocationItems", "LabelLocationCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Location", "LabelLocationCount", "DivLocationItems", "TableLocation");
            }
            else// check
            {
                SelectAllValue("Location", "DivLocationItems", "LabelLocationCount", "TableLocation");

            }
        }
    });


}


