$(document).ready(function ()
{
    $.ajax({
        url: "/AgentDashboard/ShowNotification",
        type: "POST",
        success: function (result) {
            //SUCCESS LOGIC
            $("#AgentNotification").text(result);
            pollServer();
        },
        error: function () {
            //ERROR HANDLING
            pollServer();
        }
    });

    pollServer();

});

function pollServer() {
    window.setTimeout(function () {
        $.ajax({
            url: "/AgentDashboard/ShowNotification",
            type: "POST",
            success: function (result) {
                //SUCCESS LOGIC
                $("#AgentNotification").text(result);
                pollServer();
            },
            error: function () {
                //ERROR HANDLING
                pollServer();
            }
        });
    }, 300000);

}