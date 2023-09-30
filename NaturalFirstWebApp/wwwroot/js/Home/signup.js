let waitingTime = 60;
let timer;
var code;
let isEmailVerified;

$(document).ready(function () {
    var url = window.location.href;
    //var regex = /\/SignUp\/([^/]+)/; // Matches anything after "/page/"
    var regex = /\?([^/]+)/; // Matches anything after "/page/"
    var typingTimer;
    var doneTypingInterval = 1000; // 1 second


    var match = url.match(regex);
    if (match) {
        var dynamicValue = match[1];
        $("#ReferralCode").val(dynamicValue);
    }

    $('#verification-code').on('keyup', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
    });

    function doneTyping() {
        if ($('#verification-code').val() == code) {
            alert("Email verified successfully.");
            isEmailVerified = true;
        } else {
            alert('Verification code is incorrect!');
        }
    }
});

$('#SubmitSignUp').click(function () {
    if (validate() == true) {
        SignUp();
    }
    else {
        alert('Please provide mandatory fields!')
    }
});

function sendVerificationCode() {
    let email = $('#Email').val();
    if ($('#Email').val() != null && $('#Email').val() != undefined && $('#Email').val() != "") {
        $.ajax({
            url: "/Home/SendOTP", // Replace with your API endpoint
            type: "GET",
            data: { email: $('#Email').val() },
            dataType: "json",
            success: function (response) {
                code = response;
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });

        let isButtonDisabled = false;
        if (!isButtonDisabled) {
            isButtonDisabled = true;
            $("#send-verification-btn").prop("disabled", true);

            setTimeout(function () {
                isButtonDisabled = false;
                $("#send-verification-btn").prop("disabled", false);
            }, 15000); // 60 seconds in milliseconds
        }
    } else {
        alert('Please provide email id for verification code.');
    }
}

function SignUp() {
    let _data = {
        "Email": $('#Email').val(),
        "Password": $('#Password').val(),
        "ReferralCode": $('#ReferralCode').val()
    };
    $.ajax({
        url: "/Home/SignUp", // Replace with your API endpoint
        type: "POST",
        data: { signUp : _data },
        dataType: "json",
        success: function (response) {
            if (response.statusId === 1) {
                alert(response.status);
                window.location.href = "/Home";
            }
            else
            {
                alert(response.status);
                window.location.href = "/Home";
            }
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });

    let isButtonDisabled = false;
    if (!isButtonDisabled) {
        isButtonDisabled = true;
        $(this).prop("disabled", true);

        setTimeout(function () {
            isButtonDisabled = false;
            $("#disable-button").prop("disabled", false);
        }, 15000); // 60 seconds in milliseconds
    }
}

function validate() {
    let result = true;

    if ($('#Email').val() == null || $('#Email').val() == undefined) {
        alert('Email is mandatroy!');
        result = false;
    }

    if ($('#ReferralCode').val() == null || $('#ReferralCode').val() == undefined) {
        alert('Please use a referral link to SignUp!');
        result = false;
    }

    if ($('#Password').val() === $('#confirm-password').val() && $('#Password').val() == null) {
        alert('Password is mandatroy!');
        result = false;
    }

    return result;
}
