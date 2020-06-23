/* ------------------------------------------------------------------------------
*
*  # Date and time pickers
*
*  Specific JS code additions for picker_date.html page
*
*  Version: 1.1
*  Latest update: Aug 10, 2016
*
* ---------------------------------------------------------------------------- */
function LoadDatRangePicker(startDate, endDate) {

    // Show calendars on left
    $('.daterange-left').daterangepicker({
        opens: 'left',
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        format: 'DD-MM-YYYY'
    });
    $('input[name=date-range-picker]').on('apply.daterangepicker', function (ev, picker) {
        var dateS = picker.startDate.format('YYYY-MM-DD');
        var dateE = picker.endDate.format('YYYY-MM-DD');
        $.ajax({
            url: '/Reporting/ReportManager/SaveSelectedDateInSession',
            type: 'POST',
            cache: false,
            data: { dateStart: dateS, dateEnd: dateE },
            success: function (data) {
               
            },
            error: function () {
                $("#result").text('an error occured')
            }
        });
        //LoadAJAXCallsForDashboard(picker.startDate.format('MMMM D, YYYY'), picker.endDate.format('MMMM D, YYYY'));
        //alert("apply event fired, start/end dates are " + picker.startDate.format('DD-MM-YYYY') + " to " + picker.endDate.format('DD-MM-YYYY'));
    });
}
