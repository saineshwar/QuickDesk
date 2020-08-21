
$(document).ready(function () {
    document.getElementById('timer').innerHTML = 10 + ":" + 00;
    startTimer();
});

$(document).mousemove(function (event) {
    document.getElementById('timer').innerHTML = 10 + ":" + 00;
});


$(document).on('keypress', function (e) {
    if (e.which != null) {
        document.getElementById('timer').innerHTML = 10 + ":" + 00;
    }
});


function startTimer() {
    var presentTime = document.getElementById('timer').innerHTML;
    var timeArray = presentTime.split(/[:]+/);
    var m = timeArray[0];
    var s = checkSecond((timeArray[1] - 1));
    if (s == 59) {
        m = m - 1;
    }

    if (m < 0)
    {
        var url = '/Process/AutoOutProcess';
        $(location).attr('href', url);
    }
    else
    {

        document.getElementById('timer').innerHTML =
            m + ":" + s;
        setTimeout(startTimer, 1000);
    }

}

function checkSecond(sec) {
    if (sec < 10 && sec >= 0) { sec = "0" + sec }; // add zero in front of numbers < 10
    if (sec < 0) { sec = "59" };
    return sec;
}