// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let apiUrl = "https://localhost:7131";

function convertToUppercase(inputElement) {
    inputElement.value = inputElement.value.toUpperCase();
}

function ByteArrayToImage(byteArray) {
    if (byteArray && byteArray.length > 0) {
        var binary = atob(byteArray);
        var byteArray = new Uint8Array(binary.length);
        for (var i = 0; i < binary.length; i++) {
            byteArray[i] = binary.charCodeAt(i);
        }
        var blob = new Blob([byteArray], { type: 'image/jpeg' });
        var imageUrl = URL.createObjectURL(blob);
        return '<img src="' + imageUrl + '"/>';
    } else {
        return 'No Image';
    }

    /*
    var blob = new Blob([new Uint8Array(byteArray)], {type: 'image/jpeg'});
    var imageUrl = URL.createObjectURL(blob);
    var img = new Image();
    img.src = imageUrl;
    return img;
    */
};