// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const button = document.getElementById("btn-add");
button.addEventListener("click", function () {
    $("#form_add").toggleClass("d-none");
    $("#from_search").toggleClass("d-none");
    $(this)[0].innerText = ($(this)[0].innerText == "THÊM MỚI" ? "Quay lại" : "Thêm mới");
});