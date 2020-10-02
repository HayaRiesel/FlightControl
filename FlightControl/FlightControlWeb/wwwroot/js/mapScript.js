
////map
let regularIcon;
let specialIcon;



function initializeMap() {
    map = L.map('mapid').setView([31.0461, 34.8516], 4);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoiaGF5YWxlcjg4IiwiYSI6ImNrYWdyN3N4eTA5YzAycnQ5b3lvcHl6dHkifQ.Ol4FAdl4ABHrtqMJ7czGhg'
    }).addTo(map);

    //add option to find mark by ID
    L.Map.include({
        getMarkerById: function (id) {
            let marker = null;
            //pass on all the layer and check their id
            this.eachLayer(function (layer) {
                if (layer instanceof L.Marker) {
                    if (layer.options.id === id) {
                        marker = layer;
                    }
                }
            });
            return marker;
        }
    });

    //general icon plan
    let planIcon = L.Icon.extend({
        options: {
            // size of the icon
            iconSize: [30, 30],
            // point of the icon which will correspond to marker's location
            iconAnchor: [15, 15],
        }
    });
    //the diffrent is in the color
    regularIcon = new planIcon({ iconUrl: 'img/plan.png' });
    specialIcon = new planIcon({ iconUrl: 'img/specialPlan.png' });
}

function markAllFlightMap() {
    for (let flight of allFlightList) {
        let makerIconKind = deleteFlightMark(flight.flight_id);
        markFlightMap(flight, makerIconKind);
    }
}

function markFlightMap(flight, makerIconKind) {
    let longitude = flight.longitude;
    let latitude = flight.latitude;
    let marker = L.marker([latitude, longitude], { icon: makerIconKind }).addTo(map);
    marker.addEventListener("click", function () { clickFlight(flight.flight_id); }, false);
    marker.options.id = flight.flight_id;
}

//the func return the icon of themark before the delete 
function deleteFlightMark(flightID) {
    let marker = map.getMarkerById(flightID);

    //the flight doesnt have icon (first time)
    if (marker == null) {
        return regularIcon;
    }

    let makerIconKind = marker.getIcon();
    if (marker != null) {
        map.removeLayer(marker);
    }
    return makerIconKind;
}

function specialMarkFlightMap(flightID) {
    let marker = map.getMarkerById(flightID);
    marker.setIcon(specialIcon);
}

function regularMarkFlightMap(flightID) {
    let marker = map.getMarkerById(flightID);
    marker.setIcon(regularIcon);
}


function drawCircleInTheMap(point) {
    let circle = L.circle(point, {
        color: 'red',
        fillColor: '#f03',
        fillOpacity: 0.5,
        radius: 50
    }).addTo(map);
    return circle;

}

