function AnnualAppraisalManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterStringDeSelect(filterID, "AnnualAppraisal", "DivAnnualAppraisalItems", "LabelAnnualAppraisalCount");
            }
            else // check
            {
                SingleFilterStringSelect(filterID, filterName, "AnnualAppraisal", "DivAnnualAppraisalItems", "LabelAnnualAppraisalCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("AnnualAppraisal", "LabelAnnualAppraisalCount", "DivAnnualAppraisalItems", "TableAnnualAppraisal");
            }
            else// check
            {
                SelectAllValue("AnnualAppraisal", "DivAnnualAppraisalItems", "LabelAnnualAppraisalCount", "TableAnnualAppraisal");

            }
        }
    });


}


