$(document).ready(function () {
    if (!!isSuccess && isSuccess == 'false') {
        $('#messageDiv').removeClass("alert-info");
        $('#messageDiv').addClass("alert-danger");
        $('#messageDiv').css('display', '');
    }
});


function showErrorNotification(errors) {

    var errorListHtml = $(document.createElement("ul"));
    $(errors).each(function (index, val) {
        errorListHtml.append("<li>" + val + "</li>");
    });

    const toaster = document.querySelector('.toast-placement-ex');
    $(toaster).find('.toast-body').html(errorListHtml);
    toaster.classList.add('bg-danger');
    DOMTokenList.prototype.add.apply(toaster.classList, ['top-0', 'end-0']);
    let toastPlacement = new bootstrap.Toast(toaster);
    toastPlacement.show();
}
function showSuccessNotification(errors) {

    var errorListHtml = $(document.createElement("ul"));
    $(errors).each(function (index, val) {
        errorListHtml.append("<li>" + val + "</li>");
    });

    const toaster = document.querySelector('.toast-placement-ex');
    $(toaster).find('.toast-body').html(errorListHtml);
    toaster.classList.add('bg-success');
    DOMTokenList.prototype.add.apply(toaster.classList, ['top-0', 'end-0']);
    let toastPlacement = new bootstrap.Toast(toaster);
    toastPlacement.show();
}