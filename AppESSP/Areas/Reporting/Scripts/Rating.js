function RatingManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterDeSelect(filterID, "Rating", "DivRatingItems", "LabelRatingCount");
            }
            else // check
            {
                SingleFilterSelect(filterID, filterName, "Rating", "DivRatingItems", "LabelRatingCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("Rating", "LabelRatingCount", "DivRatingItems", "TableRating");
            }
            else// check
            {
                SelectAllValue("Rating", "DivRatingItems", "LabelRatingCount", "TableRating");

            }
        }
    });


}


