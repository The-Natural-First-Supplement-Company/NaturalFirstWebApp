$('#submitButton').click(function () {
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
        contentType:'application/json',
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
});