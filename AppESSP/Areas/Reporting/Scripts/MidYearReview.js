function MidYearReviewManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterStringDeSelect(filterID, "MidYearReview", "DivMidYearReviewItems", "LabelMidYearReviewCount");
            }
            else // check
            {
                SingleFilterStringSelect(filterID, filterName, "MidYearReview", "DivMidYearReviewItems", "LabelMidYearReviewCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("MidYearReview", "LabelMidYearReviewCount", "DivMidYearReviewItems", "TableMidYearReview");
            }
            else// check
            {
                SelectAllValue("MidYearReview", "DivMidYearReviewItems", "LabelMidYearReviewCount", "TableMidYearReview");

            }
        }
    });


}


