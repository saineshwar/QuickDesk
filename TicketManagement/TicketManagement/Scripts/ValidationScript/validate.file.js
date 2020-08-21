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
        $("#divLoading").css("display", "block");
        $("#loader").css("display", "block");

        var file = getNameFromPath($(value).val());
        var extension = file.substr((file.lastIndexOf('.') + 1));
        if (value != "" && value != null) {
            var size = ValidateFileSize(value);
            var str = value.name;
            var res = str.split("_");
            var data = "_val" + res[1];

            if (Number(size) < 50 || Number(size) > 500) {
                $("#" + data).text("The size of the uploaded documents must be between 50 KB and 500 KB.");
                $("#" + value.name).val('');
            } else {
                var flag;
                if (file != null) {
                    switch (extension) {
                        case 'jpg':
                        case 'png':
                        case 'txt':
                        case 'jpeg':
                        case 'pdf':
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
                    $("#" + data).text("You can upload only jpg , jpeg ,pdf extension file");
                    $("#" + value.name).val('');
                    $("#divLoading").css("display", "none");
                    $("#loader").css("display", "none");
                    return false;
                }
                else if (ValidateFilename(value) === false) {
                    str = value.name;
                    res = str.split("_");
                    data = "_val" + res[1];
                    $("#" + data).text("Uploaded File Cannot Contains Special Character in Name");
                    $("#" + value.name).val('');
                    $("#divLoading").css("display", "none");
                    $("#loader").css("display", "none");
                    return false;
                }
                else if (ValidateFilename(value) === true) {
                    str = value.name;
                    res = str.split("_");
                    data = "_val" + res[1];
                    $("#" + data).text("");
                    $("#divLoading").css("display", "none");
                    $("#loader").css("display", "none");
                    return true;
                }
            }
        }



        $("#divLoading").css("display", "none");
        $("#loader").css("display", "none");
    }