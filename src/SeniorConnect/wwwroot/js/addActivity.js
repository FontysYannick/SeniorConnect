$(document).ready(function () {
    $('#js-search-activity-location').select2({
        ajax: {
            method: 'GET',
            url: `https://nominatim.openstreetmap.org/search?format=json&addressdetails=1&limit=5&countrycodes=nl`,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term,
                };
            },
            processResults: function (data) {
                return {
                    results: $.map(data, function (res) {
                        let address = `${res.address.road ?? ''} ${res.address.house_number ?? ''}, ${res.address.postcode ?? ''} ${res.address.municipality ?? ''}`;
                        return { id: address, text: address };
                    })
                };
            },
            cache: true
        },
        placeholder: 'Type hier de locatie...',
        minimumInputLength: 3,
        theme: 'bootstrap-5',
        templateResult: formatResult,
        templateSelection: formatSelection,
        language: {
            inputTooShort: function () {
                return 'Voer 3 of meer tekens in';
            }
        },
    });

    function formatResult(result) {
        if (!result.id) {
            return result.text;
        }
        var $result = $(
            `<span> <i class="bi bi-geo-alt-fill me-1"></i> ${result.text} </span>`
        );
        return $result;
    }

    //get the selected value
    function formatSelection(selection) {
        return selection.text;
    }
});
