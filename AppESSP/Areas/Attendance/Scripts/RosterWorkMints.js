function CalculateWorkMints() {
    var total = 0;
    var subtotal = 0;

    $("#CalculateWorkMints").click(function () {

        var noofdays = document.getElementById("noOfDays").value;

        for (i = 0; i < noofdays; i++) {

            var subtotal = document.getElementById("rows").value;
            total += parseInt(subtotal);
        }
        
        if (isNaN(total)) {

            alert("Please fill out all fields");
        }
        else {
            total = total / 60;

            return alert("Total enterd hours is " + total + ", Roster Saved");
        }
    });
}