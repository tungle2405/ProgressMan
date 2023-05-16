// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Toggle chuyển nút và ẩn hiện form thêm mới
const button = document.getElementById("btn-add");
const buttonClear = document.getElementById("lamMoi");
button.addEventListener("click", function () {
    $("#form_add").toggleClass("d-none");
    $("#from_search").toggleClass("d-none");
    $(this)[0].innerText = ($(this)[0].innerText == "THÊM MỚI" ? "Quay lại" : "Thêm mới");
});

//Nút Làm mới
buttonClear.addEventListener("click", function () {
    $('input[type=text]').each(function () {
        $(this).val('');
    });
    $('select.cchange option:disabled').prop('selected', true);
    $("input[type=radio][name=inlineRadioOptions]:first").prop('checked', true);
    $("#insertBTN").removeClass("d-none");
    $("#updateBTN").addClass("d-none");
    $("#emailAddress").prop('disabled', false);
});

//Convert từ có dấu sang không dấu
function convertViToEn(str, toUpperCase = false) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    // Some system encode vietnamese combining accent as individual utf-8 characters
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // Huyền sắc hỏi ngã nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // Â, Ê, Ă, Ơ, Ư

    return toUpperCase ? str.toUpperCase() : str;
}