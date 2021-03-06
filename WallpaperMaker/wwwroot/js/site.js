// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const documentImage = document.querySelector("#myImage");

const convertButton = document.querySelector("#convertButton");

convertButton.onclick = async function ()
{
    const apiUrl = "api/1.0/Convert";

    const baseUrl = documentImage.src;

    const response = await fetch(apiUrl + "?url=" + baseUrl);

    const blob = await response.blob();

    const objectURL = URL.createObjectURL(blob);

    documentImage.src = objectURL;

    convertButton.disabled = true;
};