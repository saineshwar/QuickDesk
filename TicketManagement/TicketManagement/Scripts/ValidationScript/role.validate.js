
$('#RoleName').blur(function () {
    if ($("#RoleName").val() !== "") {
        var url = "/RoleMaster/CheckRoleName";
        $.getJSON(url, { rolename: $("#RoleName").val() }, function (data) {
            if (data) {
                alert('RoleName already Exists');
                $("#RoleName").val('');
            }
        });
    }
});