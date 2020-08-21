$(document).ready(function ()
{
    $("#PriorityId").change(function ()
    {
        if (confirm("Are you sure you want to change Priority this?")) {
            if ($("#PriorityId").val() != "") {
                var jsonObject =
                {
                    TicketId: $("#TicketId").val(),
                    PriorityId: $("#PriorityId").val()
                };

                $.ajax({
                    type: "POST",
                    url: "../TicketDetailsHOD/ChangePriority",
                    dataType: "json",
                    data: JSON.stringify(jsonObject),
                    contentType: "application/json; charset=utf-8",
                    success: function(data, textStatus, xhr) {
                        alert("Priority Updated Successfully");
                        location.reload();
                    },
                    error: function(xhr, status, err) {
                        if (xhr.status == 400) {
                            DisplayModelStateErrors(xhr.responseJSON.ModelState);
                        }
                    }
                });
            }
        }
    });

    $("#StatusId").change(function () {

        if (confirm("Are you sure you want to change Status this?")) {
            if ($("#StatusId").val() != "") {
                var jsonObject =
                {
                    TicketId: $("#TicketId").val(),
                    StatusId: $("#StatusId").val(),
                    FirstResponseStatus: $("#FirstResponseStatus").val(),
                    ResolutionStatus: $("#ResolutionStatus").val(),
                    EveryResponseStatus: $("#EveryResponseStatus").val(),
                    EscalationStatus: $("#EscalationStatus").val(),
                    PriorityId: $("#PriorityId").val()
                };

                if ($("#StatusId").val() == '8' || $("#StatusId").val() == '9') {
                    alert('You Cannot set this status!');
                    location.reload();

                } else {
                    $.ajax({
                        type: "POST",
                        url: "../TicketDetailsHOD/ChangeStatus",
                        dataType: "json",
                        data: JSON.stringify(jsonObject),
                        contentType: "application/json; charset=utf-8",
                        success: function(data, textStatus, xhr) {
                            alert("Status Updated Successfully");
                            location.reload();
                        },
                        error: function(xhr, status, err) {
                            if (xhr.status == 400) {
                                DisplayModelStateErrors(xhr.responseJSON.ModelState);
                            }
                        }
                    });
                }
            }
        }
    });

    $("#CategoryId").change(function ()
    {
        if (confirm("Are you sure you want to change Category this?")) {
            if ($("#CategoryId").val() != "") {
                var jsonObject =
                {
                    TicketId: $("#TicketId").val(),
                    CategoryId: $("#CategoryId").val()
                };

                $.ajax({
                    type: "POST",
                    url: "../TicketDetailsHOD/ChangeCategory",
                    dataType: "json",
                    data: JSON.stringify(jsonObject),
                    contentType: "application/json; charset=utf-8",
                    success: function(data, textStatus, xhr) {
                        alert("Status Updated Successfully");
                        location.reload();
                    },
                    error: function(xhr, status, err) {
                        if (xhr.status == 400) {
                            DisplayModelStateErrors(xhr.responseJSON.ModelState);
                        }
                    }
                });
            }
        }
    });

});

function deleteTicketReply(ticketId, ticketReplyId)
{
    var result = confirm("Do you want to delete this ticket reply!");
    if (result === true) {
        if (ticketId != "" && ticketReplyId != "")
        {
            var jsonObject =
            {
                TicketId: ticketId,
                ticketReplyId: ticketReplyId
            };

            $.ajax({
                type: "POST",
                url: "../TicketDetailsHOD/Delete",
                dataType: "json",
                data: JSON.stringify(jsonObject),
                contentType: "application/json; charset=utf-8",
                success: function (data, textStatus, xhr)
                {
                    if (data === true) {
                        alert("Ticket Reply Deleted Successfully");
                        location.reload();
                    } else {
                        alert("Something Went Wrong While Deleting Ticket Please Try Again after Sometime!");
                    }
                 
                },
                error: function (xhr, status, err)
                {
                    if (xhr.status == 400)
                    {
                        DisplayModelStateErrors(xhr.responseJSON.ModelState);
                    }
                }
            });
        }
    }
}

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

function SendEmail(trackingId) {
    var result = confirm("Do you want to send email of this ticket?");
    if (result === true) {
        if (trackingId != "") {
            var jsonObject =
            {
                TrackingId: trackingId
            };

            $.ajax({
                type: "POST",
                url: "../SendNotificationAdmin/Notification",
                dataType: "json",
                data: JSON.stringify(jsonObject),
                contentType: "application/json; charset=utf-8",
                success: function (data, textStatus, xhr) {
                    if (data === true) {
                        alert("Ticket Emailed Successfully");
                        location.reload();
                    } else {
                        alert("Something Went Wrong While Emailing Ticket Please Try Again after Sometime!");
                    }

                },
                error: function (xhr, status, err) {
                    if (xhr.status == 400) {
                        DisplayModelStateErrors(xhr.responseJSON.ModelState);
                    }
                }
            });
        }
    }
}