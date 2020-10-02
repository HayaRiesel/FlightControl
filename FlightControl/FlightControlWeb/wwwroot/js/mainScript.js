

///////////////main///////////
main();


function main() {
    $("body").css("cursor", "progress");
    initializeMap();
    intilizeUploadFile();
    //update the flight by the server
    setInterval(GETAllflight, 2000);
    setInterval(clock, 1000);
}



function GETAllflight() {
    let time = new Date();
    time = time.toISOString();
    if (time.length == 24) {
        time = time.slice(0, 19) + time.slice(23);
    }
    if (time.length == 27) {
        time = time.slice(0, 22) + time.slice(26);
    }


    $.ajax({
        type: 'GET',
        url: '/api/Flights',
        data: { relative_to: time + '&sync_all' },
        success: function (data) {
            if (data.status != "fail") {
                deleteError(true);
                allFlightList = data;
                buildMyFlightTable();
            }
        },
        error: function () { // error callback 
            showErrror();
            $("body").css("cursor", "default");
        }

    });

}

function buildMyFlightTable() {
    //for notice if flight no more in the server
    makeAllcurrentIdFlightFalse();
    //add the new flight to table and update that the flight still here
    for (let flight of allFlightList) {
        if (currentIdFlight.get(flight.flight_id) == "delete") {
            continue;
        }
        if (!currentIdFlight.has(flight.flight_id)) {
            if (flight.is_external == true) {
                AddToTable(flight, "allFlightTable");
            } else {
                AddToTable(flight, "myFlightTable");
            }
        }
        currentIdFlight.set(flight.flight_id, true);
    }

    //delete the flight that is no in the server anymore
    updateCurrentIdFlight();
    //mark the flight on map
    markAllFlightMap();
    $("body").css("cursor", "default");

}


//delete the flight that no exist aymore
function updateCurrentIdFlight() {
    for (let flightID of currentIdFlight.keys()) {
        if (currentIdFlight.get(flightID) == "delete") {
            currentIdFlight.delete(flightID);
            continue;
        }
        if (currentIdFlight.get(flightID) == false) {
            //delete the flight
            deleteFlightOnTheView(flightID);
        }
    }
}

function makeAllcurrentIdFlightFalse() {
    for (let flightID of currentIdFlight.keys()) {
        if (currentIdFlight.get(flightID) == "delete") {
            continue;
        }
        currentIdFlight.set(flightID, false);
    }
}


