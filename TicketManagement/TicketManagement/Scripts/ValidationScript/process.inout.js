function processin() {
    $.ajax({
        url: "/Process/InProcess",
        type: "POST",
        dataType: "json",
        data: { values: 'Process' },
        success: function (data) {
            if (data == "OK")
            {
                alert("You have checkInn for the day Successfully");
                location.href = "/AgentDashboard/Dashboard";
            }
            else {
                alert("Error While CheckIn Please Logout and Log in Again");
            }
        }
    });
}


function processout() {
    $.ajax({
        url: "/Process/OutProcess",
        type: "POST",
        dataType: "json",
        data: { values: 'Process' },
        success: function (data)
        {
            if (data == "OK")
            {
                alert("You have checkout for the day Successfully");
                location.href = "/CheckInAlert/Alerts";
            }
            else {
                alert("Error While CheckIn Please Logout and Log in Again");
            }
        }
    });
}