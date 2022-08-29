$(document).ready(function () {
    //$(document).on('click', '#btnLogin', function (e) {
    //    submitLogin(e);
    //});
});

function submitLogin(e) {
    var errMsg = [];
    if ($('#Email').val() == null || $('#Email').val() == '') {
        errMsg.push('Email is required.');
    }
    if ($('#Password').val() == null || $('#Password').val() == '') {
        errMsg.push('Password is required.');
    }
    if (errMsg.length > 0) {
        e.preventDefault();
        showErrorNotification(errMsg);
        showSuccessNotification(errMsg);
        return;
    }
    $("#loginForm").submit();
}