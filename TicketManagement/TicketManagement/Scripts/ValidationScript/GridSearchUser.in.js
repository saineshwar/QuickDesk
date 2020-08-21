$(document).ready(function () {
    $('#GridTableContainer').jtable({
        paging: true,
        pageSize: 10,
        sorting: true,
        pageSizeChangeArea: false,
        actions:
        {
            listAction: '/UserDashboard/GetAllTickets'
        },
        recordsLoaded: function (event, data) {
            for (var i = 0; i < data.records.length; i++) {
                if (data.records[i].Priority === "Low") {
                    $('tr:has(td:contains("Low"))').css('background-color', '#eff9ff').css('color', 'black');
                }
                else if (data.records[i].Priority === "High") {
                    $('tr:has(td:contains("High"))').css('background-color', '#ffecd2').css('color', 'black');
                }
                else if (data.records[i].Priority === "Medium") {
                    $('tr:has(td:contains("Medium"))').css('background-color', '#def9ed').css('color', 'black');
                }
                else if (data.records[i].Priority === "Urgent") {
                    $('tr:has(td:contains("Urgent"))').css('background-color', '#ffeded').css('color', 'black');
                }
            }
        },
        fields: {
            TicketId: {
                key: true,
                list: false
            },
            Category: {
                title: 'Category',
                width: '10%'
            },
            TrackingId:
            {
                title: 'TrackingId',
                width: '10%',
                sorting: false,
                display: function (data) {
                    if (data.record.Priority == "Assigning Ticket..") {
                        return data.record.TrackingId;
                    } else {


                        return '<a href="javascript:void(0);" id="btntrack" onclick="Ticket(\'' +
                            data.record.TrackingId +
                            '\')" style="color: #056dc6;"> <strong>' +
                            data.record.TrackingId +
                            '</strong></a>';
                    }
                }
            },
            Name:
            {
                title: 'Name',
                width: '10%'
            },
            Subject: {
                sorting: false,
                title: 'Subject',
                width: '20%'
            },

            Priority:
            {
                title: 'Priority',
                width: '5%',

                display: function (data) {
                    if (data.record.Priority == "Low") {
                        return '<span class="label status status-closed" style="color:#1760e3;"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "Medium") {
                        return '<span class="label status status-closed" style="color:#0a9c5b"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "High") {
                        return '<span class="label status status-closed" style="color: Red;"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "Urgent") {
                        return '<span class="label status status-closed" style="color: #ff8c00;"> <strong>' +
                            data.record.Priority +
                            '</strong></span>';
                    } else {
                        return '<span class="label status status-closed">Assigning Ticket..</span>';
                    }
                }
            },
            Status: {
                title: 'Status',
                sorting: false,
                width: '9%',
                display: function (data) {
                    if (data.record.EscalationStatus == "1") {
                        return '<span class="label statusoverdue">Escalation</span>';
                    }
                    else if (data.record.FirstResponseStatus == "1" && data.record.ResolutionStatus == "1" && data.record.EveryResponseStatus == "1")
                    {
                        return '<span class="label statusoverdue">Overdue</span>';
                    }
                    else if (data.record.FirstResponseStatus == "1" && data.record.ResolutionStatus == "1" && data.record.EveryResponseStatus == "0") {
                        return '<span class="label statusoverdue">First & Resolution due</span>';
                    }
                    else if (data.record.ResolutionStatus == "1") {
                        return '<span class="label statusoverdue">Resolution Due</span>';
                    }
                    else if (data.record.EveryResponseStatus == "1") {
                        return '<span class="label statusoverdue">Every response due</span>';
                    }
                    else if (data.record.Status == "Open") {
                        return '<span class="label status status-closed">' + data.record.Status + '</span>';
                    }
                    else {
                        return '<span class="label status status-closed">' + data.record.Status + '</span>';
                    }

                }
            },
            TicketUpdatedDate: {
                title: 'Last Updated',
                sorting: true,
                width: '10%',
                display: function (data) {
                    if (data.record.TicketUpdatedDate != null) {
                        var date = new Date(parseInt(data.record.TicketUpdatedDate.substr(6)));
                        var myDate = dateFormatter(date, "date-time");
                        return myDate;
                    } else {
                        return '<span class="label status status-closed">Assigning Ticket..</span>';
                    }

                }
            }
        }
    });

    //Re-load records when user click 'load records' button.
    $('#BtnSearch').click(function (e) {
        e.preventDefault();
        $('#GridTableContainer').jtable('load', {
            search: $('#search').val(),
            searchin: $('#searchin').val()
        });
    });

    $('#BtnClear').click(function (e) {
        location.reload();
    });


    //Load Customer data from server
    $('#GridTableContainer').jtable('load');
});

function Ticket(trackingId) {
    window.location.href = '/TicketDetails/Details?TrackingId=' + trackingId;
    return false;
}

function dateFormatter(strDate, format) {
    var theDate = new Date(strDate);
    if (format == "time")
        return getTimeFromDate(theDate);
    else {
        var dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };
        var formattedDate = theDate.toLocaleDateString("en-GB", +dateOptions);
        if (format == "date")
            return formattedDate;
        return formattedDate + " " + getTimeFromDate(theDate);
    }
}

function getTimeFromDate(theDate) {
    var sec = theDate.getSeconds();
    if (sec < 10)
        sec = "0" + sec;
    var min = theDate.getMinutes();
    if (min < 10)
        min = "0" + min;
    return theDate.getHours() + ':' + min;

    //+ ':' + sec
}

var url = "/UserDashboard/GetAllSearchFields";
$.getJSON(url, function (data)
{
    if (data) {
        $("#searchin").empty();
        $.each(data, function (index, optionData)
        {
            $("#searchin").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
        });
    }
});