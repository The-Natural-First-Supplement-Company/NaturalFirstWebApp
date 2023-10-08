$(document).ready(function () {
    $("#loader").show();
    GetList();
});

function GetList() {
    $.ajax({
        url: "/Admin/GetActiveUser",
        type: "GET",
        //dataType: "json",
        async: false,
        success: function (response) {
            /*
            wbHistoryId,ProductImage,ProductName,Amount,Status,Total,ProductCount
            */
            for (var i = 0; i < response.length; i++) {
                var item = response[i];
                // Access item properties like item.wbHistoryId, item.ProductImage, etc.
                var byteArray = [response[i].profilePic];
                var imageHtml = '';
                if (response[i].profilePic) {
                    imageHtml = ByteArrayToImage(byteArray);
                } else {
                    imageHtml = 'No image';
                }
                let pageHtml = '';
                pageHtml += '<div class="profile-info"><div class="profile-info-wrapper"><div class="profile-content"><div class="profile-image" > ';
                pageHtml += '' + imageHtml + '</div><div class="profile-text"><p>';
                pageHtml += '<span id="user-mail"> User : ' + response[i].email + '</span></p>';
                pageHtml += '<span> NickName : ' + response[i].nickName + '</span></p>';
                pageHtml += '<span> Register Date : ' + convertUtcToIst(response[i].createdDate) + '</span></p>';
                pageHtml += '</div></div><div class="details-button">';
                pageHtml += '<button class="detail" id="btnSubmit' + i + '" data-id' + i + '="' + response[i].id + '" onClick="Submit(' + i + ')">Block<i class="arrow right"></i></button></div></div></div>';
                $('#ContainDiv').append(pageHtml);
            }

            $("#loader").hide();
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
};

function Submit(id){
    $('#btnSubmit' + id).attr('data-id' + id);
    //UpdateUserStatus
    let parm = {
        'Id': $('#btnSubmit' + id).attr('data-id' + id),
        'isActive':0
    };
    let josnstr = JSON.stringify(parm);
    $.ajax({
        url: "/Admin/UpdateUserStatus",
        type: "POST",
        dataType: "json",
        data: josnstr,
        //data: { "wbHistoryId": $('#btnSubmit' + id).attr('data-id' + id) },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            alert(response.status);
            $("#loader").show();
            $('#ContainDiv').html('');
            GetList();
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
};