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
    $('#processesTable tr').not(':first').remove();
    $.post('getProcesses', {}, function (data) {
        var html = '';
        data.forEach(function (process) {
            html += '<tr>' +
                '<td></td>' +
                '<td>' + process.name + '</td>' +
                '<td>' + process.path + '</td>' +
                '<td>' + process.user + '</td>' +
                '</tr>';
        });
        $('#processesTable tr').first().after(html);
    });
}

$(document).ready(function () {
    refreshProcesses();
});