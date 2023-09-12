const popup = document.querySelector('.popup');
const closeButton = document.getElementById('close-button');

closeButton.addEventListener('click', () => {
    // Close the popup when the close button is clicked
    popup.classList.remove('active');
});

function ShowPopUp(id) {
    $('#withdraw-popup').addClass('active');

    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: '/Admin/GetWithdrawById?Id=' + id,
        //data: id,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data) {
                // Populate the popup with data
                $('#rcEmail').text(data.email);
                $('#user-id').text(data.userId);
                $('#amount').html('&nbsp;&#x20B9;&nbsp;' + data.amount);
                $('#BankTxt').html(data.bank);
                $('#AccountTxt').html(data.bankAccount);
                $('#IFSCTxt').html(data.ifscCode);
                $('#trnDate').text(moment(data.createdDate).format('DD-MM-YYYY hh:mm A'));

                // Render the image using the ByteArrayToImage function
                var byteArray = [data.image];
                var imageHtml = '';
                if (data.image) {
                    imageHtml = ByteArrayToImage(byteArray);
                } else {
                    imageHtml = 'No image';
                }

                $('.popup-image').html(imageHtml);
                $('#btnDeny').attr('onclick', 'SaveRecharge(' + data.idWithdraw+ ',2)');
                $('#btnAccept').attr('onclick', 'SaveRecharge(' + data.idWithdraw + ',1)');
            }
        },
        error: function (edata) {
            alert("Error while fetching records.");
        }
    });
};

function SaveRecharge(rcId, status) {
    $.ajax({
        type: "GET", // Change from "GET" to "POST"
        contentType: "application/json; charset=utf-8",
        url: '/Admin/UpdateWithdraw?id=' + rcId + '&status=' + status, // Remove query parameters from the URL
        //data: JSON.stringify({ id: 25 }), // Send data as JSON
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.statusId === 1) {
                alert(data.status);
                $('#close-button').click();
                window.location.href = "/Admin/Withdraw";
            } else {
                alert(data.status);
            }
        },
        error: function (edata) {
            alert("Error while fetching records.");
        }
    });
};