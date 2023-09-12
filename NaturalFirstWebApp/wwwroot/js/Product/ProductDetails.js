const purchaseButton = document.querySelector('.purchase-button');
const purchasePopup = document.getElementById('purchase-popup');
const cancelButton = purchasePopup.querySelector('.cancel-button');
const confirmButton = purchasePopup.querySelector('.confirm-button');

purchaseButton.addEventListener('click', () => {
    FetchWallet();
    purchasePopup.style.display = 'block';
});

cancelButton.addEventListener('click', () => {
    purchasePopup.style.display = 'none';
});

confirmButton.addEventListener('click', () => {
    // Perform purchase action or other logic here
    PurchaseProduct();
    //purchasePopup.style.display = 'none';
});

const purchaseDiv = document.querySelector('.purchase-button');
const popUp = document.querySelector('.popup');
const closeButton = document.querySelector('.cancel-button');


purchaseDiv.addEventListener('click', () => {
    popUp.classList.toggle('active');
});

closeButton.addEventListener('click', () => {
    popUp.classList.remove('active');
});

function FetchWallet() {
    $.ajax({
        url: "/Product/GetPurchaseWallet",
        type: "GET",
        async: false,
        //data: { email: $('#Email').val() },
        dataType: "json",
        success: function (response) {
            $('#balance-value').text(response.balance);
            $('#recharge-value').text(response.recharge);
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
};

function PurchaseProduct() {
    var parm = {
        'ProductId': parseInt($('#ProductId').attr('data-Id'))
    };

    let josnstr = JSON.stringify(parm);

    $.ajax({
        url: "/Product/PurchaseProduct",
        type: "POST",
        async: false,
        data: josnstr,
        dataType: "json",
        contentType:'application/json',
        success: function (response) {
            if (response.statusId === 1) {
                alert(response.status);
                window.location.href = "/Product/Products";
            } else {
                alert(response.status);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
}