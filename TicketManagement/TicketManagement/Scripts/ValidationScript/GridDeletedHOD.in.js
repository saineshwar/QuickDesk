$(document).ready(function () {
    $('#GridTableContainer').jtable({
        paging: true,
        pageSize: 10,
        sorting: true,
        pageSizeChangeArea: false,
        selectingCheckboxes: true,
        selecting: true, //Enable selecting
        multiselect: true, //Allow multiple selecting
        actions:
        {
            listAction: '/HODDashboard/GetAllDeletedTickets'
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
            }, Category:
            {
                title: 'Category',
                sorting: true,
                width: '10%'
            },
            TrackingId:
            {
                title: 'TrackingId',
                width: '10%',
                sorting: false,
                display: function (data) {
                    return '<a href="javascript:void(0);" id="btntrack" onclick="Ticket(\'' + data.record.TrackingId + '\')" style="color: black;"> <strong>' + data.record.TrackingId + '</strong></a>';
                }
            },

            Name:
            {
                title: 'Name',
                width: '10%'
            },
            Subject: {
                title: 'Subject',
                sorting: false,
                width: '20%'
            },
            Priority:
            {
                title: 'Priority',
                width: '5%',
                display: function (data) {
                    if (data.record.Priority == "Low") {
                        return '<span style="color:#1760e3;"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "Medium") {
                        return '<span style="color:#0a9c5b"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "High") {
                        return '<span style="color: Red;"> <strong>' + data.record.Priority + '</strong></span>';
                    }
                    else if (data.record.Priority == "Urgent") {
                        return '<span style="color: #ff8c00;"> <strong>' + data.record.Priority + '</strong></span>';
                    }

                }
            },
            Status: {
                title: 'Status',
                sorting: false,
                width: '10%',
                display: function (data) {

                    if (data.record.Status == "Open") {

                        return '<span style="color: black;"> <strong>' + data.record.Status + '</strong></span>';
                    } else {

                        return '<span style="color: black;"> <strong>' + data.record.Status + '</strong></span>';
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
                        return "------";
                    }

                }
            }
        }

    });

    $('#GridTableContainer').jtable('load');

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

    $('#btnRestore').button().click(function () {
        var values = [];
        var $selectedRows = $('#GridTableContainer').jtable('selectedRows');

        $('#SelectedRowList').empty();

        if ($selectedRows.length > 0) {
            $selectedRows.each(function () {
                var record = $(this).data('record');
                values.push(record.TicketId);
            });
            processRestore(values);

        } else {
            alert("No row selected!");
        }
    });

});

var url = "/HODDashboard/GetAllSearchFields";
$.getJSON(url, function (data) {
    if (data) {
        $("#searchin").empty();
        $.each(data, function (index, optionData) {
            $("#searchin").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
        });
    }
});

var url = "/HODDashboard/GetAllPriority";
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

var url = "/HODDashboard/GetAllStatus";
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

// Change Priority in bulk
function processRestore(values) {
    $.ajax({
        type: "POST", //HTTP POST Method
        url: "/HODDashboard/RestoreTickets",
        data: { ticketlist: values },
        success: function (data) {
            if (data.status == "Success") {
                alert('Ticket Restored Successfully');
                location.reload();
            }
            else {
                alert('Restored Failed please try after sometime');
            }

        }
    });
}