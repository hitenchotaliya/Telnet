import $ from 'jquery';

$(document).ready(function () {
    getStates();
    $('#State').attr('disabled', true);
    $('#City').attr('disabled', true);

    $('#Country').change(function () {
        $('#State').attr('disabled', false);
        var id = $(this).val();
        $('#State').empty();
        $('#State').append('<option>---Select State---</option>');
        $.ajax({
            url: '/UserData/GetStatesByCountry?countryId=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#State').append('<option value=' + data.sid + '>' + data.name + ' </option>');
                });
            }
        });
    });

    $('#State').change(function () {
        $('#City').attr('disabled', false);
        var id = $(this).val();
        $('#City').empty();
        $('#City').append('<option>---Select City---</option>');
        $.ajax({
            url: '/UserData/GetCitiesByState?stateId=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#City').append('<option value=' + data.cid + '>' + data.name + ' </option>');
                });
            }
        });
    });
});

function getStates() {
    $.ajax({
        url: '/UserData/GetStates',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#State').append('<option value=' + data.sid + '>' + data.name + ' </option>');
            });
        }
    });
}
