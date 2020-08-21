$(document).ready(function () {
    SetProfile();
    $("#bntDeleteProfile").click(function () {
        deleteprofile();
    });

});

function SetProfile() {
    $.ajax({
        type: "POST",
        url: '/AgentAdmin/CheckIsProfileImageExists',
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
                url: '/AgentAdmin/Profileimage',
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
            url: '/AgentAdmin/DeleteProfilepicture',
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



function SetSignature() {
    $.ajax({
        type: "POST",
        url: '/AgentAdmin/GetSignature',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.result === true) {
                $("#TextSignature").val(data.values);
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

function UpdateSignature() {
    if ($("#TextSignature").val() != "") {
        var obj = { Signature: $("#TextSignature").val() };
        $.ajax({
            type: "POST",
            url: '/AgentAdmin/UpdateSignature',
            data: JSON.stringify(obj),
            contentType: "application/json;",
            success: function (data) {
                if (data.result === true) {
                    alert("Signature Updated Successfully");
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
}
