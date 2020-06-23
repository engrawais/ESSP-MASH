function LoadDatRangePicker(startDate, endDate) {

    $('.daterange-left').daterangepicker({
        opens: 'left',
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        format: 'DD-MM-YYYY'
    });
    // Show calendars on left
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