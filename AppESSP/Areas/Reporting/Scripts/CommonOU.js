function CommonOUManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID,"CommonOU", "DivCommonOUItems", "LabelCommonOUCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "CommonOU", "DivCommonOUItems", "LabelCommonOUCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("CommonOU", "LabelCommonOUCount", "DivCommonOUItems", "TableCommonOU");
            }
            else// check
            {
                SelectAllValue("CommonOU", "DivCommonOUItems", "LabelCommonOUCount","TableCommonOU");
                
            }
        }
    });


}


