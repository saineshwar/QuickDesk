$.ajax({
    type: "POST",
    url: "/AllRoles/GetAllRoles",
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (data)
    {
        $.each(data, function (i)
        {
            var optionhtml = '<option value="' + data[i].Value + '">' + data[i].Text + '</option>';
            $("#RoleID").append(optionhtml);
            $("#DropRoles").append(optionhtml);
        });
    }
});




function validate()
{
    if ($("#RoleID").val() == "-1") {
        return false;
    } else {
        return true;
    }
}