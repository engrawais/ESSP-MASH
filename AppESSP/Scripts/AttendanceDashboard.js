function LoadDateRangePicker() {

    $('.daterange-left').daterangepicker({
        locale: {
            format: 'DD/MM/YYYY'
        },
        opens: 'left',
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        //format: 'DD-MM-YYYY',
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, function (start, end, label) {
        // Save Values in sessions
        $.ajax({
            url: '/AttendanceDashboard/SaveSelectedDateInSession',
            type: 'POST',
            cache: false,
            data: { dateStart: start.format('YYYY-MM-DD'), dateEnd: end.format('YYYY-MM-DD') },
            success: function (data) {
                var urls = '/AttendanceDashboard/LoadPieChart';
                $.ajax({
                    url: urls,
                    type: "GET",
                    cache: false,
                }).done(function (result) {
                    $("#divLoading").hide();
                    $('#PVCTMSParent').html(result);
                });
            },
            error: function () {
                $("#result").text('an error occured')
            }
        });
    });

}
function LoadDateRangePickerForTODashboard() {

    $('.daterange-left').daterangepicker({
        opens: 'left',
        applyClass: 'bg-slate-600',
        cancelClass: 'btn-default',
        format: 'DD-MM-YYYY',
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, function (start, end, label) {
        // Save Values in sessions
        $.ajax({
            url: '/AttendanceDashboard/SaveSelectedDateInSession',
            type: 'POST',
            cache: false,
            data: { dateStart: start.format('YYYY-MM-DD'), dateEnd: end.format('YYYY-MM-DD') },
            success: function (data) {
                location.reload();
            },
            error: function () {
                $("#result").text('an error occured')
            }
        });
    });

}
// Execute first time when MainContainer View Render
// Load default Graph accoridng to user access rights
function OpenPieChart(model) {
    if (model.UserGraphType == "1") {
        //AddActiveClassToButton(model.GraphType);
        //LoadAttendance(model.EmpID);
        $(window).attr('location', '/ESSP/ESSPAttendence/Index')
    }
    if (model.UserGraphType == "2") {
        LoadPieChart(model.GraphType);
    }
    if (model.UserGraphType == "3") {
        LoadPieChart(model.GraphType);
    }
    if (model.UserGraphType == "4") {
        LoadPieChart(model.GraphType);
    }
}

function SaveBtnEventsInSession(grpahType, UserGraphType, EmpID) {
    $.ajax({
        url: '/AttendanceDashboard/SaveBtnEventsInSession',
        type: "GET",
        cache: false,
        data: { GraphType: grpahType }
    }).done(function (result) {
        if (UserGraphType == 'Single') {
            AddActiveClassToButton(grpahType);
            LoadAttendance(EmpID);
        }
        else {
            LoadPieChart(grpahType);
        }
    });
}
function SaveLabelEventsInSession(id) {
    $.ajax({
        url: '/AttendanceDashboard/SaveLabelEventsInSession',
        type: "GET",
        cache: false,
        data: { id: id }
    }).done(function (result) {
        LoadPieChart(result);
    });
}
function LoadPieChart(graphtype) {
    AddActiveClassToButton(graphtype)
    var urls = '/AttendanceDashboard/LoadPieChart';
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCTMSParent').html(result);
    });
}
function AddActiveClassToButton(graphtype) {
    $('#btnClickLO').removeClass('active').addClass('');
    $('#btnClickLI').removeClass('active').addClass('');
    $('#btnClickEO').removeClass('active').addClass('');
    $('#btnClickEI').removeClass('active').addClass('');
    $('#btnClickAB').removeClass('active').addClass('');
    $('#btnClickLV').removeClass('active').addClass('');
    $('#btnClickOD').removeClass('active').addClass('');
    switch (graphtype) {
        case 'LateIn':
            $('#btnClickLI').addClass('active');
            break;
        case 'LateOut':
            $('#btnClickLO').addClass('active');
            break;
        case 'EarlyIn':
            $('#btnClickEI').addClass('active');
            break;
        case 'EarlyOut':
            $('#btnClickEO').addClass('active');
            break;
        case 'Absent':
            $('#btnClickAB').addClass('active');
            break;
        case 'Leave':
            $('#btnClickLV').addClass('active');
            break;
        case 'Leave':
            $('#btnClickOD').addClass('active');
            break;
    }
}
function RenderPieChartTMS(obj2) {
    //google.charts.load("current", { packages: ["corechart"] });
    //google.charts.setOnLoadCallback(drawChart);
    //function drawChart() {
    //    var myArray = [];
    //    myArray.push(["Criteria", 'No of Employees']);
    //    var obj = jQuery.parseJSON(obj2)
    //    for (i = 0; i < obj.length; i++) {
    //        myArray.push([obj[i].Name, obj[i].Count]);
    //    }
    //    var data = google.visualization.arrayToDataTable(myArray);

    //    var options = {
    //        fontSize: 11,
    //        //title: 'My Daily Activities',
    //        height: 400,
    //        pieHole: 0.2,
    //    };

    //    var chart = new google.visualization.PieChart(document.getElementById('pieChart'));
    //    chart.draw(data, options);
    //}
    // Set paths
    // ------------------------------

    require.config({
        paths: {
            echarts: 'assets/js/plugins/visualization/echarts'
        }
    });


    // Configuration
    // ------------------------------

    require(
        [
            'echarts',
            'echarts/theme/limitless',
            'echarts/chart/pie',
            'echarts/chart/funnel'
        ],


        // Charts setup
        function (ec, limitless) {


            // Initialize charts
            // ------------------------------

            var basic_pie = ec.init(document.getElementById('basic_pie'), limitless);


            // Charts setup
            // ------------------------------                    

            //
            // Basic pie options
            //
            var myArray = [];
            var obj = jQuery.parseJSON(obj2)
            for (i = 0; i < obj.length; i++) {
                var item = { value: obj[i].Count, name: obj[i].Name };
                myArray.push(item);
            }
            basic_pie_options = {

                // Add title
                //title: {
                //    text: 'Browser popularity',
                //    subtext: 'Open source information',
                //    x: 'center'
                //},

                // Add tooltip
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b}: {c} ({d}%)"
                },

                // Add legend


                // Display toolbox
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        magicType: {
                            show: true,
                            title: {
                                pie: 'Switch to pies',
                                funnel: 'Switch to funnel',
                            },
                            type: ['pie', 'funnel'],
                            option: {
                                funnel: {
                                    x: '25%',
                                    y: '20%',
                                    width: '50%',
                                    height: '70%',
                                    funnelAlign: 'left',
                                    max: 1548
                                }
                            }
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Enable drag recalculate
                calculable: true,

                // Add series
                series: [{
                    name: 'Browsers',
                    type: 'pie',
                    radius: '50%',
                    center: ['50%', '57.5%'],
                    data: myArray
                }]
            };


            //
            // Infographic donut options
            //

            // Data style
            var dataStyle = {
                normal: {
                    label: { show: false },
                    labelLine: { show: false }
                }
            };

            // Placeholder style
            var placeHolderStyle = {
                normal: {
                    color: 'rgba(0,0,0,0)',
                    label: { show: false },
                    labelLine: { show: false }
                },
                emphasis: {
                    color: 'rgba(0,0,0,0)'
                }
            };



            //
            // Pie timeline options
            //

            var idx = 1;


            //
            // Multiple donuts options
            //

            // Top text label
            var labelTop = {
                normal: {
                    label: {
                        show: true,
                        position: 'center',
                        formatter: '{b}\n',
                        textStyle: {
                            baseline: 'middle',
                            fontWeight: 100,
                            fontSize: 10
                        }
                    },
                    labelLine: {
                        show: false
                    }
                }
            };

            // Format bottom label
            var labelFromatter = {
                normal: {
                    label: {
                        formatter: function (params) {
                            return '\n\n' + (100 - params.value) + '%'
                        }
                    }
                }
            }

            // Bottom text label
            var labelBottom = {
                normal: {
                    color: '#eee',
                    label: {
                        show: false,
                        position: 'center',
                        textStyle: {
                            baseline: 'middle'
                        }
                    },
                    labelLine: {
                        show: false
                    }
                },
                emphasis: {
                    color: 'rgba(0,0,0,0)'
                }
            };

            // Set inner and outer radius
            var radius = [60, 75];




            // Apply options
            // ------------------------------

            basic_pie.setOption(basic_pie_options);



            // Resize charts
            // ------------------------------

            window.onresize = function () {
                setTimeout(function () {
                    basic_pie.resize();
                }, 200);
            }
        }
    );
}

function LoadAttendance(id) {
    var urls = '/AttendanceDashboard/LoadEmployeeAttendance';
    $.ajax({
        url: urls,
        type: "GET",
        cache: false,
        data: { id: id }
    }).done(function (result) {
        $("#divLoading").hide();
        $('#PVCTMSParent').html(result);
    });
}