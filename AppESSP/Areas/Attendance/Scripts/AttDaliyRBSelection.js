    $("#ShiftDiv").hide();
    $("#LocDiv").hide();
    $("#CrewDiv").hide();
    $("#DivDiv").hide();
    $("#DeptDiv").hide();
    $("#SecDiv").hide();
    $("#EmpDiv").hide();
    var test = $("input[name$='RosterSelectionRB']:checked").val();
    if (test == "rbAll") {
        $("div.desc").hide();
    }
    if (test == "rbShift") {
        $("div.desc").hide();
        $("#ShiftDiv").show();
    } if (test == "rbLocation") {
        $("div.desc").hide();
        $("#LocDiv").show();
    }
    if (test == "rbGroup") {
        $("div.desc").hide();
        $("#CrewDiv").show();
    }
    if (test == "rbDivision") {
        $("div.desc").hide();
        $("#DivDiv").show();
    }
    if (test == "rbDepartment") {
        $("div.desc").hide();
        $("#DeptDiv").show();
    }
    if (test == "rbSection") {
        $("div.desc").hide();
        $("#SecDiv").show();
    }
    if (test == "rbEmployee") {
        $("div.desc").hide();
        $("#EmpDiv").show();
    }
    $("input[name$='RosterSelectionRB']").click(function () {
        var test = $(this).val();
        if (test == "rbAll") {
            $("div.desc").hide();
        }
        if (test == "rbShift") {
            $("div.desc").hide();
            $("#ShiftDiv").show();
        }
        if (test == "rbLocation") {
            $("div.desc").hide();
            $("#LocDiv").show();
        }
        if (test == "rbGroup") {
            $("div.desc").hide();
            $("#CrewDiv").show();
        }
        if (test == "rbDivision") {
            $("div.desc").hide();
            $("#DivDiv").show();
        }
        if (test == "rbDepartment") {
            $("div.desc").hide();
            $("#DeptDiv").show();
        }
        if (test == "rbSection") {
            $("div.desc").hide();
            $("#SecDiv").show();
        }
        if (test == "rbEmployee") {
            $("div.desc").hide();
            $("#EmpDiv").show();
        }
    });