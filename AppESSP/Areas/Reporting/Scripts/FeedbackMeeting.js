function FeedbackMeetingManager() {
    $('input[type=checkbox]').change(function () {
        if ($(this).closest('tr').find('.chkSelect').val()) // if Select All is not clicked
        {
            var filterID = $(this).closest('tr').find('.chkSelect').val();
            var filterName = $(this).closest("tr").find(".valSelect").text();
            if (this.checked == false) // uncheck
            {
                SingleFilterStringDeSelect(filterID, "FeedbackMeeting", "DivFeedbackMeetingItems", "LabelFeedbackMeetingCount");
            }
            else // check
            {
                SingleFilterStringSelect(filterID, filterName, "FeedbackMeeting", "DivFeedbackMeetingItems", "LabelFeedbackMeetingCount");
            }
        }
        else  // if Select All clicked
        {
            if (this.checked == false) // uncheck
            {
                RemoveAllValues("FeedbackMeeting", "LabelFeedbackMeetingCount", "DivFeedbackMeetingItems", "TableFeedbackMeeting");
            }
            else// check
            {
                SelectAllValue("FeedbackMeeting", "DivFeedbackMeetingItems", "LabelFeedbackMeetingCount", "TableFeedbackMeeting");

            }
        }
    });


}


