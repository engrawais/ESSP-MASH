function OpenPartialView(myUrl,heading) {
    $.ajax({
        url: myUrl,
        type: 'GET',
        cache: false,
        success: function (data) {
            $('#DivContainer').html(data);
            $('#LBReportHeading').html(heading);
        },
        error: function () {
            $("#result").text('an error occured')
        }
    });
}
function MakeFilterSelectBox(filterID, filtername, filtertype, divitems, labelcount) {
    var parent = document.getElementById(divitems);
    var newChild1 = "<div class='label border-left-info label-striped full-width' id= '" + divitems + "-" + filterID + "' >";
    var newChild2 = '<button type="button" onClick="SingleFilterDeSelect(' + filterID + ',\'' + filtertype + '\',\'' + divitems + '\',\'' + labelcount + '\')"';
    var newChild3 = " class='btn btn-link pull-left'>" + filtername + "<i class='icon-cross3 position-right'></i ></button ></div >";
    var newchild4 = "<div class='label border-left-info label-striped full-width'></div>";
    var newChild = newChild1.concat(newChild2, newChild3, newchild4);
    parent.insertAdjacentHTML('beforeend', newChild);
}
//"+filterID+", '"+filterType + "', '" + divItems + "', '" + labelCount + "'
function SingleFilterSelect(filterID, filtername, filtertype, divitems, labelcount) {
    $.ajax({
        url: '/Reporting/ReportManager/SaveValueInSession',
        type: 'POST',
        cache: false,
        data: { id: filterID, name: filtername, type: filtertype },
        success: function (data) {
            //Add new label under selected filter expended area
            MakeFilterSelectBox(filterID, filtername, filtertype, divitems, labelcount);

            if (document.getElementById(labelcount).innerHTML == null) {
                document.getElementById(labelcount).innerHTML = 1;
            }
            else {
                document.getElementById(labelcount).innerHTML = parseInt(document.getElementById(labelcount).innerHTML) + 1;
            }
        },
        error: function () {
        }
    });
}
function SingleFilterDeSelect(filterid, filtertype, divitems, labelcount) {
    $.ajax({
        url: '/Reporting/ReportManager/RemoveValueFromSession',
        type: 'POST',
        cache: false,
        data: { id: filterid, type: filtertype },
        success: function (data) {
            document.getElementById(labelcount).innerHTML = parseInt(document.getElementById(labelcount).innerHTML) - 1;
            DeleteFilterHTML(divitems, filterid);
            var tableID = "#Table" + filtertype;
            $(tableID).find('tr').each(function () {
                var row = $(this);
                if (row.find('.chkSelect').val() == filterid) {
                    row.find('.chkSelect').prop('checked', false);
                }
            });
        },
        error: function () {

        }
    });
}

function RemoveAllValues(filtertype, labelcount, divitems, tableid) {
    $.ajax({
        url: '/Reporting/ReportManager/RemoveAllValueFromSession',
        type: 'POST',
        cache: false,
        data: { type: filtertype },
        success: function (data) {
            document.getElementById(labelcount).innerHTML = 0;
            document.getElementById(divitems).innerHTML = '';
            $("#" + tableid).find('tr').each(function () {
                var row = $(this);
                row.find('.chkSelect').prop('checked', false);
            });
        },
        error: function () {

        }
    });
}
function SelectAllValue(filtertype, divitems, labelcount, tableid) {
    $.ajax({
        url: '/Reporting/ReportManager/SaveAllValueInSession',
        type: 'POST',
        cache: false,
        data: { type: filtertype },
        success: function (data) {
            document.getElementById(divitems).innerHTML = '';
            document.getElementById(labelcount).innerHTML = document.getElementById(tableid).rows.length - 1;
            $("#" + tableid).find('tr').each(function () {
                var row = $(this);
                row.find('.chkSelect').prop('checked', true);
                if (row.find('.chkSelect').val()) {
                    MakeFilterSelectBox(row.find('.chkSelect').val(), row.find('.valSelect').text(), filtertype, divitems, labelcount);
                }
            });
        },
        error: function () {

        }
    });
}

function DeleteFilterHTML(filtertype, id) {
    var ElementId = filtertype + "-" + id;
    document.getElementById(ElementId).remove();
}

// Filters sfor String ID
function SingleFilterStringSelect(filterID, filtername, filtertype, divitems, labelcount) {
    $.ajax({
        url: '/Reporting/ReportManager/SaveValueInSessionString',
        type: 'POST',
        cache: false,
        data: { id: filterID, name: filtername, type: filtertype },
        success: function (data) {
            //Add new label under selected filter expended area
            MakeFilterSelectBox(filterID, filtername, filtertype, divitems, labelcount);

            if (document.getElementById(labelcount).innerHTML == null) {
                document.getElementById(labelcount).innerHTML = 1;
            }
            else {
                document.getElementById(labelcount).innerHTML = parseInt(document.getElementById(labelcount).innerHTML) + 1;
            }
        },
        error: function () {
        }
    });
}
function SingleFilterStringDeSelect(filterid, filtertype, divitems, labelcount) {
    $.ajax({
        url: '/Reporting/ReportManager/RemoveValueFromSessionString',
        type: 'POST',
        cache: false,
        data: { id: filterid, type: filtertype },
        success: function (data) {
            document.getElementById(labelcount).innerHTML = parseInt(document.getElementById(labelcount).innerHTML) - 1;
            DeleteFilterHTML(divitems, filterid);
            var tableID = "#Table" + filtertype;
            $(tableID).find('tr').each(function () {
                var row = $(this);
                if (row.find('.chkSelect').val() == filterid) {
                    row.find('.chkSelect').prop('checked', false);
                }
            });
        },
        error: function () {

        }
    });
}

