// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const documentImage = document.querySelector("#myImage");

const convertButton = document.querySelector("#convertButton");

convertButton.onclick = async function ()
{
    const apiUrl = "/Convert";

    const baseUrl = documentImage.src;

    const response = await fetch(apiUrl + "?url=" + baseUrl);

    const blob = await response.blob();

    const objectURL = URL.createObjectURL(blob);

    documentImage.src = objectURL;

    convertButton.disabled = true;
};

documentImage.onclick = function ()
{
    const imageURL = URL.createObjectURL(documentImage.src);

    documentImage.src = null;

    window.open("https://stackoverflow.com/questions/27798126/how-to-open-the-newly-created-image-in-a-new-tab", "_blank");
};