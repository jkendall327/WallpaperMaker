// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var convertButton = document.getElementById("convertButton");

convertButton.onclick = function ()
{
    var apiUrl = "/Convert";
    var documentImage = document.getElementById("myImage");

    var baseUrl = documentImage.src;

    fetch(apiUrl + "?url=" + baseUrl)
        .then(response => response.blob())
        .then(function (myBlob) {
            var objectURL = URL.createObjectURL(myBlob);
            documentImage.src = objectURL;
        });

    convertButton.disabled = true;
};