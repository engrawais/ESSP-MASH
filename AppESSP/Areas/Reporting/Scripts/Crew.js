function CrewManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Crew", "DivCrewItems", "LabelCrewCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Crew", "DivCrewItems", "LabelCrewCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Crew", "LabelCrewCount", "DivCrewItems", "TableCrew");
            }
            else// check
            {
                SelectAllValue("Crew", "DivCrewItems", "LabelCrewCount", "TableCrew");

            }
        }
    });


}


