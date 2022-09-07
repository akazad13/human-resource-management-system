$(document).ready(function () {
    $(document).on('click', '#addNewWorkHistory', function () {
        getAddEditWorkHistoryModal(0);
    });
    $(document).on('click', '#btnSaveWrokHistory', function () {
        saveWrokHistory();
    });
});

function getAddEditWorkHistoryModal(workHxId) {
    $.ajax({
        url: employeeUrl.AddEditWorkHistoryModal,
        type: 'GET',
        cache: false,
        data: {
            employeeId: $('#EmployeeId').val(),
            workHxId: workHxId
        },
        success: function (partialView) {
            $('#addEditWorkHxModalDiv').html(partialView);
            $('#addEditWorkHxModal').modal('show');
        }
    });
}
function saveWrokHistory() {
    $.ajax({
        url: employeeUrl.AddEditWorkHistoryModal,
        type: 'POST',
        cache: false,
        data: {
            Id: $('#Id').val(),
            EmployeeId: $('#EmployeeId').val(),
            EffectiveDate: $('#EffectiveDate').val(),
            Designation: $('#Designation').val(),
            ManagerId: $('#ManagerId').val(),
            ChangedType: $('#ChangedType').val(),
            Reason: $('#Reason').val(),
        },
        success: function (res) {
            
            if (res.status == 'success') {
                showSuccessNotification(res.message);
                setTimeout(() => {
                    location.reload();
                }, 2000);
            } else {
                showErrorNotification(res.message);
            }

        }
    });
}