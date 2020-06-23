function OpenNotification() {
    $.ajax({
        type: "GET",
        url: '/Notification/GetSystemNotification',
        success: function (response) {
            $("#DivNotifications").append(response.Notification);
            document.getElementById("divNotificationCount").innerHTML = response.NotificationCount;
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function LoadToasterMessage(SessionValue) {
    if (SessionValue != '[]') {
        var Message = JSON.parse(SessionValue);
        if (Message != null) {
            for (var i = 0; i < Message.length; i++) {
                $.jGrowl(Message[i], {
                    header: '',
                    theme: 'bg-primary',
                    position: 'center',
                    font: 'bolder',
                    life: 4500
                });
            }
        }
    }
}