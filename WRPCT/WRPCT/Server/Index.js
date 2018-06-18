function showError() {
    alert('Произошла ошибка при отправке запроса.');
}

function updateMinutes(positive) {
    var amount = positive ? $('#amount').val() : -$('#amount').val();
    $.post('update', { amount }, function (data) {
        location.reload();
    }).fail(function () {
        showError();
        location.reload();
    });
}

function enableGames() {
    $.post('allowgames', {}, function (data) {
        location.reload();
    }).fail(function () {
        showError();
        location.reload();
    });
}

function disableGames() {
    $.post('disallowgames', {}, function (data) {
        location.reload();
    }).fail(function () {
        showError();
        location.reload();
    });
}

function refreshProcesses() {
    $('#processSelect').empty();
}

$(document).ready(function () {
    refreshProcesses();
});