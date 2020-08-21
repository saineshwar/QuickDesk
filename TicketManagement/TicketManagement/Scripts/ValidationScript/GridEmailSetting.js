$(document).ready(function () {
    $('#GridTableContainer').jtable({
        paging: true,
        pageSize: 10,
        sorting: true,
        pageSizeChangeArea: false,
        actions:
        {
            listAction: '/SuperAdminConfiguration/GetAllSmtpSettings'
        },
        fields: {
            SmtpProviderId: {
                key: true,
                list: false
            },
            Name: {
                title: 'SettingName',

                width: '10%',
                display: function (data)
                {
                    return '<strong>' +
                        data.record.Name +
                        '</strong>';
                }
            },
            Host: {
                title: 'Host',
                sorting: false,
                width: '5%'
            },
            Port: {
                title: 'Port',
                sorting: false,
                width: '5%'
            },
            Timeout: {
                title: 'Timeout',
                sorting: false,
                width: '5%'
            },
            SslProtocol:
            {
                title: 'SslProtocol',
                sorting: false,
                width: '10%'
            },
            TlSProtocol: {
                title: 'TlSProtocol',
                sorting: false,
                width: '10%'
            },
            Username: {
                title: 'Username',
                sorting: false,
                width: '10%'
            },
            IsDefault: {
                title: 'IsDefault',
                sorting: false,
                width: '5%',
                type: 'checkbox',
                values: { 'false': 'No', 'true': 'Yes' },
                defaultValue: 'false'
            },
            Edit: {
                title: 'Action',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a class="btn btn-primary btn-xs" href="../SuperAdminConfiguration/EditSmtpSettings?id=' + data.record.SmtpProviderId + ' ">' + '<i class="glyphicon glyphicon-edit"></i> Edit </a>';
                }
            },
            SetDefault: {
                title: 'SetDefault',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a class="btn btn-primary btn-xs"  href="javascript:void(0);" onclick="SetDefault(\'' +
                        data.record.SmtpProviderId +
                        '\');">' +
                        '<i class="glyphicon glyphicon-ok"></i> Set </a>';

                }
            },
            TestConnection:
            {
                title: 'TestConnection',
                sorting: false,
                width: '5%',
                display: function (data) {
                    return '<a class="btn btn-primary btn-xs"  href="javascript:void(0);" onclick="TestConnection(\'' + data.record.SmtpProviderId + '\');">' + ' <i class="glyphicon glyphicon-signal"></i> Test </a>';
                }
            }
        }
    });

    $('#BtnSearch').click(function (e) {
        e.preventDefault();
        $('#GridTableContainer').jtable('load',
            {
                search: $('#search').val()
            });
    });

    $('#BtnClear').click(function (e) {
        location.reload();
    });

    $('#GridTableContainer').jtable('load');
});

function SetDefault(value) {
    $.ajax({
        type: "POST", //HTTP POST Method
        url: "/SuperAdminConfiguration/SetDefaultConnection",
        data: { SmtpProviderId: value },
        success: function (data) {
            if (data.status == "Success") {
                alert('Default Connection is Updated Successfully');
                location.reload();
            }
            else {
                alert('Error Occured while Setting Default Connection');
            }

        }
    });
}

function TestConnection(value) {
    $.ajax({
        type: "POST", //HTTP POST Method
        url: "/SuperAdminConfiguration/TestConnection",
        data: { SmtpProviderId: value },
        success: function (data) {
            if (data.status == "Success") {
                alert('Connection Working Properly');
                location.reload();
            }
            else {
                alert('Testing Failed. Check Email Settings');
            }
        }
    });
}
