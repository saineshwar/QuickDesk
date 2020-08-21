$(document).ready(function() {
    $("#btnActivities").click(function() {

        if ($("#TicketId").val() != "") {
            var jsonObject =
            {
                TicketId: $("#TicketId").val()
            };

            $.ajax({
                type: "POST",
                url: "/TicketHistory/Activities",
                dataType: "html",
                data: JSON.stringify(jsonObject),
                contentType: "application/json; charset=utf-8",
                success: function(data, textStatus, xhr) {
                    if (data.length != 0) {

                        $("#Tickethistory").html(data);
                        $('#myModal').modal('show');
                    }
                },
                error: function(xhr, status, err) {
                    if (xhr.status == 400) {
                        DisplayModelStateErrors(xhr.responseJSON.ModelState);
                    }
                }
            });
        }
    });

});

function DisplayModelStateErrors(modelState) {
    var message = "";
    var propStrings = Object.keys(modelState);

    $.each(propStrings, function (i, propString) {
        var propErrors = modelState[propString];
        $.each(propErrors, function (j, propError) {
            message += propError;
        });
        message += "\n";
    });

    alert(message);
};




