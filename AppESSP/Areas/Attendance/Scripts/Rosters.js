function RosterCreate (){
    //$("#cyclesDiv").hide();

    var rosterDays = 0;
    var cycles = 1;
    var rosterType = $("#RosterTypeID").val();
    switch (rosterType) {
        case '1':
            cycles = 1;
            //$("#cyclesDiv").hide();
            break;
        case '2':
            cycles = 1;
            document.getElementById("DateEnded").disabled = '';
            //$("#cyclesDiv").show();
            rosterDays = 7;
            break;
        case '3':
            cycles = 1;
            //$("#cyclesDiv").hide();
            rosterDays = 15;
            break;
        case '4':
            cycles = 1;
            //$("#cyclesDiv").hide();
            //document.getElementById("DateEnded").disabled = '';
            rosterDays = 30;
            break;
        case '5':
            cycles = 1;
            //$("#cyclesDiv").show();
            rosterDays = 84;
            break;
    }
    $('#RosterTypeID').change(function () {
        var rosterType = $(this).val();
        switch (rosterType) {
            case '1':
                cycles = 1;
                //$("#cyclesDiv").hide();
                break;
            case '2':
                cycles = 1;
                document.getElementById("DateEnded").disabled = '';
                rosterDays = 7;
                break;
            case '3':
                cycles = 1;
                //$("#cyclesDiv").hide();
                rosterDays = 15;
                break;
            case '4':
                cycles = 1;
                //document.getElementById("DateEnded").disabled = 'false';
                rosterDays = 30;
                break;
            case '5':
                cycles = 1;
                //$("#cyclesDiv").show();
                rosterDays = 84;
                break;
        }
        //$("#cycles").val(cycles);
        if (document.getElementById('DateStarted').value) {
            refreshEndDate();
        }
    });

    $('#DateStarted').change(function () {
        var rosterType = $("#RosterTypeID").val();
        if (rosterType == 4) {
            refreshEndDate();
        }
    });

    //$('#cycles').change(function () {
    //    refreshEndDate();
    //});


    function refreshEndDate() {
        var dateStartedString = document.getElementById('DateStarted').value;
        var dateStart = new Date(dateStartedString);
        var month = dateStart.getMonth()+1;
        var daysinMonth = daysInMonth(month, dateStart.getFullYear())
        var DateEnd = dateStart;
        DateEnd.setDate(dateStart.getDate() + daysinMonth-1);
        
        //var someFormattedDate = y+ '-' + mm + '-' + dd;
        document.getElementById('DateEnded').value = DateEnd.toISOString().substring(0, 10);;
        document.getElementById('dateEndHidden').value = document.getElementById('DateEnded').value;
    }

    function daysInMonth(month, year) {
        return new Date(year, month, 0).getDate();
    }

};

function RosterContinue(id) {
        $.ajax({
            type: "GET",
            url: "/Attendance/Roster/RosterContinue",
            contentType: "application/json; charset=utf-8",
            data: { "id": id},
            datatype: "json",
            success: function (url) {
                $('#modelBody').html(url);
                $('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
}