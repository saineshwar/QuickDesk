$(document).ready(function () {
    $('#GridTableContainer').jtable({
        title: 'All tickets',
        pageSize: 10,
        paging: true,
        sorting: true,
        pageSizeChangeArea: true,
        selectingCheckboxes: true,
        selecting: true, //Enable selecting
        multiselect: true, //Allow multiple selecting
        actions:
        {
            listAction: '/AgentAdminDashboard/GetAllAgentAdminTickets'
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
            Category:
            {
                title: 'Category',
                sorting: false
            },
            TrackingId:
            {
                title: 'TrackingId',
                width: '10%',
                sorting: false,
                display: function (data) {
                    return '<a href="javascript:void(0);" id="btntrack" onclick="Ticket(\'' + data.record.TrackingId + '\')" style="color: #056dc6;"> <strong>' + data.record.TrackingId + '</strong></a>';
                }
            },
            Name:
            {
                title: 'Name'
            },

            Subject: {
                sorting: false,
                title: 'Subject'

            },
            Priority:
            {
                sorting: false,
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
                        return '<span class="label status status-closed" style="color: #ff8c00;"> <strong>' + data.record.Priority + '</strong></span>';
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
                    else if (data.record.FirstResponseStatus == "1") 
                    {
                        return '<span class="label statusoverdue">First response due</span>';
                    }
                    else if (data.record.ResolutionStatus == "1") 
                    {
                        return '<span class="label statusoverdue">Resolution Due</span>';
                    }
                    else if (data.record.EveryResponseStatus == "1")
                    {
                        return '<span class="label statusoverdue">Every response due</span>';
                    }
                    else if (data.record.Status == "Open")
                    {
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
                width: '12%',
                display: function (data) {
                    if (data.record.TicketUpdatedDate != null) {
                        var date = new Date(parseInt(data.record.TicketUpdatedDate.substr(6)));
                        var myDate = dateFormatter(date, "date-time");
                        return myDate;
                    } else {
                        return "------";
                    }

                }
            }
        }
    });

    //Re-load records when user click 'load records' button.
    $('#BtnSearch').click(function (e) {
        e.preventDefault();
        $('#GridTableContainer').jtable('load',
            {
                search: $('#search').val(),
                searchin: $('#searchin').val(),
                prioritysearch: $('#DropPriority').val(),
                statussearch: $('#DropStatus').val()
            });
    });
    $('#BtnClear').click(function (e) {
        location.reload();
    });

    //Load Customer data from server
    $('#GridTableContainer').jtable('load');

    $('#BtnChangeAllPriority').click(function () {
        var values = [];
        var $selectedRows = $('#GridTableContainer').jtable('selectedRows');

        $('#SelectedRowList').empty();

        if ($selectedRows.length > 0) {
            if ($("#DropChangePriority").val() != '') {
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    values.push(record.TicketId);
                });

                processChangeAllPriority(values);
            } else {
                alert("No row selected!");
            }
        } else {
            alert("No row selected!");
        }
    });


    $('#BtnChangeAllStatus').click(function () {
        var values = [];
        var $selectedRows = $('#GridTableContainer').jtable('selectedRows');

        $('#SelectedRowList').empty();

        if ($selectedRows.length > 0) {
            if ($("#DropChangeStatus").val() != '') {
                $selectedRows.each(function () {
                    var record = $(this).data('record');
                    values.push(record.TicketId);
                });

                ProcessChangeAllStatus(values);
            } else {
                alert("No row selected!");
            }
        } else {
            alert("No row selected!");
        }
    });




    // AutoComplete for Usernames
    $("#Username").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/AgentAdminDashboard/GetAllAgentUsers",
                type: "GET",
                dataType: "json",
                data: { usernames: request.term },
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            return { label: item.Username, value: item.UserId };
                        }));
                }
            });
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        select: function (e, i) {
            $("#Username").val(i.item.label);
            $("#hdUserReferenceId").val(i.item.value);
            return false;
        },
        minLength: 1
    });

    $("#BtnAssign").click(function () {
        if ($("#Username").val() == "" || $("#hdUserReferenceId").val() == "") {
            alert("Select Suggested Names");
        } else {
            AssignUserTicket();
        }
    });
});

function Ticket(trackingId) {
    window.location.href = '/TicketDetailsAgentsAdmin/Details?TrackingId=' + trackingId;
    return false;
}

// Change Priority in bulk
function processChangeAllPriority(values) {
    $.ajax({
        type: "POST", //HTTP POST Method
        url: "/AgentAdminDashboard/ProcessChangeAllPriority",
        data: { ticketlist: values, priority: $("#DropChangePriority").val() },
        success: function (data) {
            if (data.status == "Success") {
                alert('Ticket Priority Updated Successfully');
                location.reload();
            }
            else {
                alert('Records Not Updated');
            }

        }
    });
}

// Change Status in bulk
function ProcessChangeAllStatus(values) {
    $.ajax({
        type: "POST", //HTTP POST Method
        url: "/AgentAdminDashboard/ProcessChangeAllStatus",
        data: { ticketlist: values, status: $("#DropChangeStatus").val() },
        success: function (data) {
            if (data.status == "Success") {
                alert('Ticket Status Updated Successfully');
                location.reload();
            }
            else {
                alert('Records Not Updated');
            }

        }
    });
}

function AssignUserTicket() {
    var values = [];
    var $selectedRows = $('#GridTableContainer').jtable('selectedRows');

    $('#SelectedRowList').empty();

    if ($selectedRows.length > 0) {

        $selectedRows.each(function () {
            var record = $(this).data('record');
            values.push(record.TicketId);
        });

        if (values != "") {
            $.ajax({
                type: "POST", //HTTP POST Method
                url: "/AgentAdminDashboard/AssignTickettoUser",
                data: { ticketlist: values, userId: $("#hdUserReferenceId").val() },
                success: function (data) {
                    if (data.status == "Success") {
                        alert('Ticket Status Updated Successfully');
                        location.reload();
                    }
                    else {
                        alert('Records Not Updated');
                    }

                }
            });
        }


    } else {
        alert("No row selected!");
    }
}

var url = "/AgentAdminDashboard/GetAllSearchFields";
$.getJSON(url, function (data) {
    if (data) {
        $("#searchin").empty();
        $.each(data, function (index, optionData) {
            $("#searchin").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
        });
    }
});

var url = "/AgentAdminDashboard/GetAllPriority";
$.getJSON(url, function (data) {
    if (data) {
        $("#DropPriority").empty();
        $("#DropChangePriority").empty();

        $.each(data, function (index, optionData) {
            $("#DropPriority").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
            $("#DropChangePriority").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
        });
    }
});

var url = "/AgentAdminDashboard/GetAllStatus";
$.getJSON(url, function (data) {
    if (data) {
        $("#DropStatus").empty();
        $("#DropChangeStatus").empty();

        $.each(data, function (index, optionData) {
            $("#DropStatus").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
            $("#DropChangeStatus").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
        });
    }
});

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