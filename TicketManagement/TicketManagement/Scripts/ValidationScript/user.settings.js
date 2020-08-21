$(document).ready(function () {
    SetProfile();
    $("#bntDeleteProfile").click(function () {
        deleteprofile();
    });

});

function SetProfile() {
    $.ajax({
        type: "POST",
        url: '/User/CheckIsProfileImageExists',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result === true) {
                $("#img_profile").hide();
                $("#img_profile_uploaded").show();

                $("#uploadedprofile").attr('src', data.base64string);
            }
            else {
                $("#img_profile").show();
                $("#img_profile_uploaded").hide();
            }
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });
}

$("#li3").click(function () {
    SetSignature();
});

$("#btnSignature_OK").click(function () {
    UpdateSignature();
});

function Uploadprofilepicture(value) {
    var files = value.files;
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var fd = new FormData();

            for (var x = 0; x < files.length; x++) {
                fd.append("file" + x, files[x]);
            }

            $.ajax({
                type: "POST",
                url: '/User/Profileimage',
                contentType: false,
                processData: false,
                data: fd,
                success: function (result) {
                    alert("Updated profile picture Successfully");
                    SetProfile();
                    $("#li2").addClass("active");
                },
                error: function (xhr, status, p3, p4) {
                    var err = "Error " + " " + status + " " + p3 + " " + p4;
                    if (xhr.responseText && xhr.responseText[0] == "{")
                        err = JSON.parse(xhr.responseText).Message;
                    console.log(err);
                }
            });
        } else {
            alert("This browser doesn't support HTML5 file uploads!");
        }
    }
}

function deleteprofile() {
    var result = confirm("Do you want to delete profile picture!");
    if (result == true) {
        $.ajax({
            type: "POST",
            url: '/User/DeleteProfilepicture',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.result === true) {
                    $("#li2").addClass("active");

                    SetProfile();
                    $("#profileimageupload_1").val('');
                } else {
                    alert("Error While Delete Profile Picture Please try after sometime.");
                }
            },
            error: function (xhr, status, p3, p4) {
                var err = "Error " + " " + status + " " + p3 + " " + p4;
                if (xhr.responseText && xhr.responseText[0] == "{")
                    err = JSON.parse(xhr.responseText).Message;
                console.log(err);
            }
        });

    } else {

    }
}

function ValidateFilename(value) {
    var file = getNameFromPath($(value).val());
    var extension = file.split('.');
    var tempregx = new RegExp(/^[A-Za-z0-9 ]+$/);
    if (!tempregx.test(extension[0])) {
        return false;
    }
    else {
        return true;
    }
}

function ValidateFileSize(fileid) {
    try {
        var fileSize = 0;
        fileSize = $(fileid)[0].files[0].size;
        fileSize = parseFloat(fileSize / 1024).toFixed(2);
        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
    }
}

function getNameFromPath(strFilepath) {
    var objRe = new RegExp(/([^\/\\]+)$/);
    var strName = objRe.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

function ValidateFile(value) {
    $("#_val1").text("");
    var file = getNameFromPath($(value).val());
    var extension = file.substr((file.lastIndexOf('.') + 1));
    if (value != "" && value != null) {
        var size = ValidateFileSize(value);
        var str = value.name;
        var res = str.split("_");
        var data = "_val" + res[1];

        if (Number(size) < 10 || Number(size) > 500) {
            $("#" + data).text("The size of the uploaded documents must be between 50 KB and 500 KB.");
            $("#" + value.name).val('');
        } else {
            var flag;
            if (file != null) {
                switch (extension) {
                    case 'jpg':
                    case 'jpeg':
                    case 'png':
                        flag = true;
                        break;
                    default:
                        flag = false;
                }
            }
            if (flag === false) {
                str = value.name;
                res = str.split("_");
                data = "_val" + res[1];
                $("#" + data).text("You can upload only jpg , jpeg ,png extension file");
                $("#" + value.name).val('');
                return false;
            }
            else if (ValidateFilename(value) === false) {
                str = value.name;
                res = str.split("_");
                data = "_val" + res[1];
                $("#" + data).text("Uploaded File Cannot Contains Special Character in Name");
                $("#" + value.name).val('');
                return false;
            }
            else if (ValidateFilename(value) === true) {
                str = value.name;
                res = str.split("_");
                data = "_val" + res[1];
                $("#" + data).text("");
                Uploadprofilepicture(value);
                return true;
            }

        }
    }

}

