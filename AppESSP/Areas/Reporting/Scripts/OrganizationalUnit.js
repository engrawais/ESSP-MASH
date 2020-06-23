function OrganizationalUnitManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "OrganizationalUnit", "DivOrganizationalUnitItems", "LabelOrganizationalUnitCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "OrganizationalUnit", "DivOrganizationalUnitItems", "LabelOrganizationalUnitCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("OrganizationalUnit", "LabelOrganizationalUnitCount", "DivOrganizationalUnitItems", "TableOrganizationalUnit");
            }
            else// check
            {
                SelectAllValue("OrganizationalUnit", "DivOrganizationalUnitItems", "LabelOrganizationalUnitCount", "TableOrganizationalUnit");

            }
        }
    });


}


