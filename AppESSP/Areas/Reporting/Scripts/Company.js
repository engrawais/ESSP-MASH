function CompanyManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Company", "DivCompanyItems", "LabelCompanyCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Company", "DivCompanyItems", "LabelCompanyCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Company", "LabelCompanyCount", "DivCompanyItems", "TableCompany");
            }
            else// check
            {
                SelectAllValue("Company", "DivCompanyItems", "LabelCompanyCount", "TableCompany");

            }
        }
    });


}


