$(function () {
    $('[data-bs-toggle="tooltip"]').tooltip()
});
$(document).ready(function () {
    $('#SaveMode').click(function () {
        var newDateVal = $('#NewPubDate').val();
        if (newDateVal.length == 0) {
            $('#dateRequired').modal('show')
            $('#SaveMode').html($('#SaveMode').data("enabled-text"));
            return false;
        }
    });
});
