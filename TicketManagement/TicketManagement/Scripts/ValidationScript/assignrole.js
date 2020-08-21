

$('#MenuId').change(function ()
{
    if ($("#MenuId").val() !== "")
    {
        var urlSource = '/AssignRolestoSubMenu/GetSubMenuList';
        $.ajax({
            url: urlSource,
            method: "POST",
            data: {
                "menuid": $("#MenuId").val()
            },
            success: function (result) {
                $('#checkboxContainer').empty();

                var content = "";
                for (var i = 0; i < result.length; i++) {
                    content += '<input type="checkbox" class="checkboxes flat" value="' + result[i].Value + '" name="[' + [i] + '].Checked" />' + ' ' + result[i].Text + '</br>';
                    content += '<input type="hidden" name="[' + [i] + '].Text"  value="' + result[i].Text + '" />';
                    content += '<input type="hidden" name="[' + [i] + '].Value" value="' + result[i].Value + '" />';
                }

                $('#checkboxContainer').html(content);
            }
        });

    }
});


function rebindSubmenu(subMenuId)
{
    if ($("#MenuId").val() != "-1" && $("#MenuId").val() != "0" && $("#SubMenuId").val() == "-1") {
        var url = "/AssignRolestoMenu/GetSubMenuList";
        $.getJSON(url, { menuid: $("#MenuId").val() }, function (data) {
            if (data) {
                $("#SubMenuId").empty();
               
                $.each(data, function (index, optionData) {
                    $("#SubMenuId").append("<option text='" + optionData.Text + "' value='" + optionData.Value + "'>" + optionData.Text + "</option>");
                });

                $("#SubMenuId").val(subMenuId);
            }
        });
    }
}


