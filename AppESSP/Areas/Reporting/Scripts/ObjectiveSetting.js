function ObjectiveSettingManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterStringDeSelect(filterID, "ObjectiveSetting", "DivObjectiveSettingItems", "LabelObjectiveSettingCount");
            }
            else // check
            {
                SingleFilterStringSelect(filterID, filterName, "ObjectiveSetting", "DivObjectiveSettingItems", "LabelObjectiveSettingCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("ObjectiveSetting", "LabelObjectiveSettingCount", "DivObjectiveSettingItems", "TableObjectiveSetting");
            }
            else// check
            {
                SelectAllValue("ObjectiveSetting", "DivObjectiveSettingItems", "LabelObjectiveSettingCount", "TableObjectiveSetting");

            }
        }
    });


}


