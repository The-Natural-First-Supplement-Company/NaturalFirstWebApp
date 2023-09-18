let waitingTime = 60;
let timer;

$(document).ready(function () {
    $('#wait-time').hide();
});

$('#BtnSendMail').click(function () {
    sendVerificationCode();
});

function sendVerificationCode() {
    // Disable the button temporarily and start the timer
    const sendVerificationBtn = document.getElementById('BtnSendMail');
    sendVerificationBtn.disabled = true;
    timer = setInterval(updateWaitingTime, 1000);

    $.ajax({
        url: "/Home/SendOTP?email=" + $('#Email').text(),
        type: "GET",
        dataType: "json",
        success: function (response) {
            code = response;
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
}

function updateWaitingTime() {
    const waitTimeElement = document.getElementById('wait-time');
    if (waitingTime > 0) {
        waitingTime--;
        if (waitTimeElement.hidden == true) {
            waitTimeElement.hidden = false;
        }
        waitTimeElement.textContent = `Please wait ${waitingTime} seconds for the next verification code.`;
    } else {
        // Enable the button and reset the waiting time
        const sendVerificationBtn = document.getElementById('BtnSendMail');
        sendVerificationBtn.disabled = false;
        waitingTime = 60;
        clearInterval(timer);
        waitTimeElement.textContent = '';
    }
}