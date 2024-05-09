// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {
    // Event listener for country selection
    $('#country').change(function () {
        var countryId = $(this).val();
        $('#region').empty();
        $('#locality').empty();
        $('#street').empty();
        $('#addressDetails').hide();

        if (countryId) {
            $.ajax({
                url: '/GetRegions', // Change with your controller endpoint
                type: 'GET',
                data: { countryId: countryId },
                success: function (data) {
                    // Populate the regions dropdown
                    $('#region').append($('<option>', {
                        value: '',
                        text: 'Select Region'
                    }));
                    $.each(data, function (index, region) {
                        $('#region').append($('<option>', {
                            value: region.id,
                            text: region.name
                        }));
                    });
                    $('#regionContainer').show();
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching regions:', error);
                }
            });
        }
    });

    // Event listener for region selection
    $('#region').change(function () {
        var regionId = $(this).val();
        $('#locality').empty();
        $('#street').empty();
        $('#addressDetails').hide();

        if (regionId) {
            $.ajax({
                url: '/GetLocalities',
                type: 'GET',
                data: { regionId: regionId },
                success: function (data) {
                    $('#locality').append($('<option>', {
                        value: '',
                        text: 'Select Locality'
                    }));
                    $.each(data, function (index, locality) {
                        $('#locality').append($('<option>', {
                            value: locality.id,
                            text: locality.name
                        }));
                    });
                    $('#localityContainer').show();
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching localities:', error);
                }
            });
        }
    });

    // Event listener for locality selection
    $('#locality').change(function () {
        var localityId = $(this).val();
        $('#street').empty();
        $('#addressDetails').hide();

        if (localityId) {
            $.ajax({
                url: '/GetStreets', // Adjust controller endpoint
                type: 'GET',
                data: { localityId: localityId },
                success: function (data) {
                    $('#street').append($('<option>', {
                        value: '',
                        text: 'Select Street'
                    }));
                    $.each(data, function (index, street) {
                        $('#street').append($('<option>', {
                            value: street.id,
                            text: street.name
                        }));
                    });
                    $('#streetContainer').show();
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching streets:', error);
                }
            });
        }
    });

    // Event listener for street selection
    $('#street').change(function () {
        var streetId = $(this).val();
        if (streetId) {
            $('#addressDetails').show(); // Show additional address fields
        }
    });
});
// Write your JavaScript code.
