$("#CategoryName").blur(function () {
    if ("" !== $("#CategoryName").val()) {
        $.getJSON("/Category/CheckCategoryName",
            { categoryName: $("#CategoryName").val() },
            function(a) { a && (alert("CategoryName already Exists"), $("#CategoryName").val("")) });
    }
});