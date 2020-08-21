function OnlyNumeric(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode < 48 || charCode > 57)) {
        if (charCode == 8 || charCode == 46 || charCode == 0) {
            return true;
        }
        else {
            return false;
        }
    }
    return false;
}


function onlyspecchar(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var txt = String.fromCharCode(charCode)
    if ((txt.match(/^[a-zA-Z\b. ]+$/)) || (txt.match(/^[0-9]+$/)) || (charCode == 64) || (charCode == 45) || (charCode == 46) || (charCode == 95) || (charCode == 41)) {
        return true;
    }
    return false;
}