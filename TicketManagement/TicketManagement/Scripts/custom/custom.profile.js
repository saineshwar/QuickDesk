function profile(profileid) 
{
    if (profileid != "")
    {
        var jsonObject =
        {
            profileId: profileid
        };

        $.ajax({
            type: "POST",
            url: "/Profile/Details",
            dataType: "html",
            data: JSON.stringify(jsonObject),
            contentType: "application/json; charset=utf-8",
            success: function (data, textStatus, xhr) {
                if (data.length != 0) {

                    $("#divuserprofile").html(data);
                   

                    $.ajax({
                        type: "POST",
                        url: '/Profile/CheckIsProfileImageExists',
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(jsonObject),
                        success: function (data) {
                            if (data.result === true) {
                                $("#uploadedprofile").attr('src', data.base64string);



                            }
                        },
                        error: function (xhr, status, p3, p4) {
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                        }
                    });

                    $('#profileModal').modal('show');
                }
            },
            error: function (xhr, status, err) {
                if (xhr.status == 400) {
                    DisplayModelStateErrors(xhr.responseJSON.ModelState);
                }
            }
        });

       

     

      
    }
}

function DisplayModelStateErrors(modelState) {
    var message = "";
    var propStrings = Object.keys(modelState);

    $.each(propStrings, function (i, propString) {
        var propErrors = modelState[propString];
        $.each(propErrors, function (j, propError) {
            message += propError;
        });
        message += "\n";
    });

    alert(message);
};

