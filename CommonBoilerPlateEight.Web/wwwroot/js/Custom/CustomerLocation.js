
function initGeneralMap(mapId, autoCompleteInputId,latitude,longitude) {
    var location = { lat: latitude, lng: longitude };
   
    var mapOptions = {
        center: location,
        zoom: 10
    };

    var map = new google.maps.Map(document.getElementById(mapId), mapOptions);
    var input = document.getElementById(autoCompleteInputId);
    var autocomplete = new google.maps.places.Autocomplete(input, {
        componentRestrictions: { country: 'kw' }
    });

    var marker = new google.maps.Marker({
        map: map,
        draggable: true,
        position: location
    });

    autocomplete.addListener('place_changed', function () {
        onPlaceChanged(autocomplete, map, marker);
    });

    google.maps.event.addListener(marker, 'dragend', function (event) {
        var latLng = event.latLng;
        fetchLocationInfo(latLng, true);
    });
}


// Function to handle place change from the autocomplete input
function onPlaceChanged(autocomplete, map, marker) {
    var place = autocomplete.getPlace();
    if (!place.geometry) {
        window.alert("No details available for input: '" + place.name + "'");
        return;
    }

    if (place.geometry.viewport) {
        map.fitBounds(place.geometry.viewport);
    } else {
        map.setCenter(place.geometry.location);
        map.setZoom(17);
    }
    marker.setPosition(place.geometry.location);
    updateLocationInfo(place.geometry.location, [place]);

    if (!place.address_components.some(component => component.types.includes('neighborhood') && component.long_name.includes('Block'))) {
        fetchLocationInfo(place.geometry.location);
    }
}

function fetchLocationInfo(latLng, populateFullAddress = false) {
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'location': latLng }, function (results, status) {
        if (status === 'OK') {
            if (results && results.length > 0) {
                updateLocationInfo(latLng, results, populateFullAddress);
            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });
}



function updateLocationInfo(latLng, results, populateFullAddress = false) {
    $(".Latitude").val(latLng.lat());
    $(".Longitude").val(latLng.lng());

    let addressInfo = {
        area: '',
        state: '',
        street: '',
        block: '',
        googlePlusCode: ''
    };

    results.forEach(function (result) {
        if (result.address_components) {
            result.address_components.forEach(function (component) {
                if (component.types.includes('sublocality') || component.types.includes('locality')) {
                    addressInfo.area = component.long_name;
                } else if (component.types.includes('administrative_area_level_1')) {
                    addressInfo.state = component.long_name;
                } else if (component.types.includes('country')) {
                    addressInfo.country = component.long_name;
                } else if (component.types.includes('administrative_area_level_2')) {
                    addressInfo.city = component.long_name;
                } else if (component.types.includes('route')) {
                    addressInfo.street = component.long_name;
                } else if (component.types.includes('street_number')) {
                    addressInfo.street = `${component.long_name} ${addressInfo.street}`;
                } else if (component.types.includes('neighborhood') && component.long_name.includes('Block')) {
                    addressInfo.block = component.long_name;
                } else if (component.types.includes('plus_code')) {
                    addressInfo.googlePlusCode = component.long_name;
                }
            });
        }
    });
    $(".Governorate").val(addressInfo.state);
    $(".Area").val(addressInfo.area);
    $(".Street").val(addressInfo.street);
    $(".Block").val(addressInfo.block);
    $(".GooglePlusCode").val(addressInfo.googlePlusCode);
    if (populateFullAddress) {
        $(".fullAddress").val(results[0].formatted_address);
    }

}

