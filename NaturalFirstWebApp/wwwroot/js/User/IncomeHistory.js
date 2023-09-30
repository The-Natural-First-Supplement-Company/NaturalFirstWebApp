$(document).ready(function () {
    $("#loader").show();
    GetHistory();
});

function GetHistory() {
    $.ajax({
        url: "/User/GetIncomeHistory",
        type: "GET",
        //dataType: "json",
        async: false,
        success: function (response) {
            /*
            wbHistoryId,ProductImage,ProductName,Amount,Status,Total,ProductCount
            */
            if (response.length > 0) {
                if (response[0].wbHistoryId == 0 || response == null) {
                    $("#loader").hide();
                } else {
                    // Iterate through the result here
                    for (var i = 0; i < response.length; i++) {
                        var item = response[i];
                        // Access item properties like item.wbHistoryId, item.ProductImage, etc.
                        var byteArray = [response[i].productImage];
                        var imageHtml = '';
                        if (response[i].productImage) {
                            imageHtml = ByteArrayToImage(byteArray);
                        } else {
                            imageHtml = 'No image';
                        }
                        let pageHtml = '';
                        pageHtml += '<div class="profile-info"><div class="profile-info-wrapper"><div class="profile-content"><div class="profile-image" > ';
                        pageHtml += '' + imageHtml + '</div><div class="profile-text"><p>';
                        pageHtml += '<span id="user-mail">Product : ' + response[i].productName + '</span></p><p><span id="user-id">Earning : ' + response[i].amount + '</span></p>';
                        pageHtml += '<span id="user-mail">Remarks : ' + response[i].remarks + '</span></p>';
                        pageHtml += '</div></div><div class="details-button">';
                        if (response[i].wdStatus == 1) {
                            pageHtml += '<button class="detail" disabled>Recieved</button></div></div></div>';
                        } else {
                            pageHtml += '<button class="detail-danger" disabled>Missed</button></div></div></div>';
                        }

                        $('#ContainDiv').append(pageHtml);
                        $("#loader").hide();
                    }
                }
            } else {
                let pageHtml = '';
                pageHtml = '<div style="color:black;text-align:center;"><p>No records.</p></div>'
                $('#ContainDiv').append(pageHtml);
                $("#loader").hide();
            }

        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
}