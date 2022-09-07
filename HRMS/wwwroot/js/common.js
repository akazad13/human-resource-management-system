$(document).ready(function () {
    if (typeof (isSuccess) != 'undefined' && isSuccess == 'false') {
        $('#messageDiv').removeClass("alert-info");
        $('#messageDiv').addClass("alert-danger");
        $('#messageDiv').css('display', '');
    }
    else if (typeof (isSuccess) != 'undefined' && isSuccess == 'true') {
        $('#messageDiv').removeClass("alert-info");
        $('#messageDiv').addClass("alert-success");
        $('#messageDiv').css('display', '');
    }
});

$(document).ajaxSend(function (event, xhr, settings) {
    try {
        var requestVerificationToken = $('#hrmsRequestVerificationToken').val();
        if (requestVerificationToken) {
            xhr.setRequestHeader('RequestVerificationToken', requestVerificationToken);
        }
    }
    catch (err) {
    }
});


function showErrorNotifications(errors) {

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
function showSuccessNotifications(messages) {

    var messageHtml = $(document.createElement("ul"));
    $(messages).each(function (index, val) {
        messageHtml.append("<li>" + val + "</li>");
    });

    const toaster = document.querySelector('.toast-placement-ex');
    $(toaster).find('.toast-body').html(messageHtml);
    toaster.classList.add('bg-success');
    DOMTokenList.prototype.add.apply(toaster.classList, ['top-0', 'end-0']);
    let toastPlacement = new bootstrap.Toast(toaster);
    toastPlacement.show();
}

function showErrorNotification(error) {

    const toaster = document.querySelector('.toast-placement-ex');
    $(toaster).find('.toast-body').html(error);
    toaster.classList.add('bg-danger');
    DOMTokenList.prototype.add.apply(toaster.classList, ['top-0', 'end-0']);
    let toastPlacement = new bootstrap.Toast(toaster);
    toastPlacement.show();
}
function showSuccessNotification(message) {
    const toaster = document.querySelector('.toast-placement-ex');
    $(toaster).find('.toast-body').html(message);
    toaster.classList.add('bg-success');
    DOMTokenList.prototype.add.apply(toaster.classList, ['top-0', 'end-0']);
    let toastPlacement = new bootstrap.Toast(toaster);
    toastPlacement.show();
}


