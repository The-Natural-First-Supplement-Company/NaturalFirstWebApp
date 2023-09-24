$('#submitButton').click(function () {
    if (validateForm() === true) {
        SubmitForm();
    }
});

function SubmitForm() {
    let parm = {
        'TrnPassword': $('#TrnPassword').val(),
        'Amount': $('#Amount').val()
    };

    let josnstr = JSON.stringify(parm);

    $.ajax({
        url: "/User/MakeWithdraws",
        type: "Post",
        data: josnstr,
        dataType: "json",
        contentType: 'application/json',
        success: function (response) {
            if (response.statusId === 1) {
                alert(response.status);
                window.location.href = "/User/Index";
            } else {
                alert(response.status);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
};

function validateForm() {
    let result = true;
    let msg = '';
    if ($('#TrnPassword').val() == '' || $('#TrnPassword').val() == null) {
        $('#TrnPassword').addClass("blank-field");
        msg += "Please provide transaction password.\n";
        result = false;
    } else {
        $('#TrnPassword').removeClass("blank-field");
    }

    if ($('#Amount').val() == '' || $('#Amount').val() == null) {
        $('#Amount').addClass("blank-field");
        msg += "Please provide amount.\n";
        result = false;
    } else {
        $('#Amount').removeClass("blank-field");
    }

    if ($('#Amount').val() < 140) {
        msg += "Withdrawal amount cannot be less than 140.";
        result = false;
    }
    if (msg != '') {
        alert(msg);
    }
    return result;
}