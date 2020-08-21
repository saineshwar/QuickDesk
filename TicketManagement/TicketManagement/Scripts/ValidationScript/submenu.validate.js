
$('#SubMenuName').blur(function () {
    if ($("#SubMenuName").val() !== "") {
        var url = "/SubMenuMaster/CheckSubMenuName";
        $.getJSON(url, { menuName: $("#SubMenuName").val(), menuID: $("#MenuId").val() }, function (data)
        {
            if (data)
            {
                alert('SubMenuName already Exists');
                $("#SubMenuName").val('');
            }
        });
    }
});