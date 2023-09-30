$(document).ready(function () {
    $("#loader").show();
    GetInformation();
});

function GetInformation() {
    $.ajax({
        url: "/User/GetPendingIncome",
        type: "GET",
        //dataType: "json",
        async: false,
        success: function (response) {
           /*
           wbHistoryId,ProductImage,ProductName,Amount,Status,Total,ProductCount
           */
            if (response.length > 0) {
                if (response[0].wbHistoryId == 0) {
                    $('#AssetTotal').text(response[0].productCount);
                    $('#TotalCost').text(response[0].total);
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
                        pageHtml += '<span id="user-mail">' + response[i].productName + '</span></p><p><span id="user-id">Earning : ' + response[i].amount + '</span></p>';
                        pageHtml += '</div></div><div class="details-button">';
                        pageHtml += '<button class="detail" id="btnSubmit' + i + '" data-id' + i + '="' + response[i].wbHistoryId + '" onClick="Submit(' + i + ')">Recieve<i class="arrow right"></i></button></div></div></div>';
                        $('#ContainDiv').append(pageHtml);
                    }
                    $('#AssetTotal').text(response[0].productCount);
                    $('#TotalCost').text(response[0].total);
                    $("#loader").hide();
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
};

function Submit(id) {
    //let val = $('#btnSubmit'+id).attr("data-id"+id);
    //alert("Code Pending For This Opton.");
    let val = $('#btnSubmit' + id).attr('data-id' + id);
    //let parm = {
    //    "wbHistoryId": $('#btnSubmit' + id).attr('data-id' + id)
    //};
    //let josnstr = JSON.stringify(parm);
    $.ajax({
        url: "/User/UpdateStatus?HistoryId="+val,
        type: "POST",
        dataType: "json",
        //data: josnstr,
        //data: { "wbHistoryId": $('#btnSubmit' + id).attr('data-id' + id) },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            alert(response.status);
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });

};
