
let numOfPressed = 0;


function handleClickOnTheDoc() {
    numOfPressed++;
    cancelClick(crrentPresedFlight.rowID);

}



function deleteExtrDetails() {
    document.getElementById("pasengers").innerHTML = "";
    document.getElementById("company_name").innerHTML = "";
    document.getElementById("start_point").innerHTML = "";
    document.getElementById("end_point").innerHTML = "";
    document.getElementById("start_time").innerHTML = "";
    document.getElementById("end_time").innerHTML = "";

}

function changeExtrDetails(flightPlan) {
    document.getElementById("pasengers").innerHTML = "&#128106" + "passengers: " + flightPlan.passengers;
    document.getElementById("company_name").innerHTML = "&#9992" + "company name: " + flightPlan.company_name;
    document.getElementById("start_point").innerHTML = "&#127757" + "start point: (" + flightPlan.initial_location.latitude + ", " + flightPlan.initial_location.longitude + ")";
    document.getElementById("end_point").innerHTML = "&#127757" + "end point:  (" + flightPlan.segments[flightPlan.segments.length - 1].latitude + "," + flightPlan.segments[flightPlan.segments.length - 1].longitude + ")";
    document.getElementById("start_time").innerHTML = "&#128336" + "start time: " + (new Date(flightPlan.initial_location.date_time)).toISOString();
    document.getElementById("end_time").innerHTML = "&#128336" + "end time: " + getEndTime(flightPlan).toISOString();
}

function cancelClick(rowID) {
    document.getElementById(rowID).className = "";
    //make the plan no red
    regularMarkFlightMap(rowID);
    //delete extra details
    deleteExtrDetails();
    //remove the path on map
    if (crrentPresedFlight.pathOnMap != null) {
        map.removeLayer(crrentPresedFlight.pathOnMap);
    }
    for (let point of crrentPresedFlight.pointsOnMap) {
        map.removeLayer(point);
    }
    crrentPresedFlight = "";
    document.removeEventListener("click", handleClickOnTheDoc, true);

}


function drawPathOnMap(flightPlan, flightID) {
    //draw the path on map
    let pointArray = [];
    let circleOnMap = [];
    let initialPoint = [flightPlan.initial_location.latitude, flightPlan.initial_location.longitude];
    pointArray.push(initialPoint);
    const initialCircle = drawCircleInTheMap(initialPoint);
    circleOnMap.push(initialCircle);
    //pass on al the segment
    for (let segment of flightPlan.segments) {
        let currentPoint = [segment.latitude, segment.longitude];
        const circle = drawCircleInTheMap(currentPoint);
        circleOnMap.push(circle);
        pointArray.push(currentPoint);
    }
    const linesOnMap = L.polyline(pointArray).addTo(map);

    let allTheFlightDetail = new presedFlight(flightID, linesOnMap, circleOnMap);
    return allTheFlightDetail;

}


function clickFlight(flightID) {

    //change color of the row
    const rowFlight = document.getElementById(flightID);
    rowFlight.className = "table-warning text-dark";
    //change the color of the plan icon
    specialMarkFlightMap(flightID);

    //zoom on the flight
    const flight = findFlightById(flightID);
    map.setView([flight.latitude, flight.longitude]);


    crrentPresedFlight = new presedFlight(flightID);
    const thisNumPress = numOfPressed;

    document.addEventListener("click", handleClickOnTheDoc, true);


    let myUrl = 'api/FlightPlan/' + flightID;

    $.ajax({
        type: 'GET',
        url: myUrl,
        success: function (data) {
            if (thisNumPress != numOfPressed) {
                return;
            }
            if (data.status != "fail") {
                deleteError(true);
                let flightPlan = data;

                //change the extra details
                changeExtrDetails(flightPlan);

                crrentPresedFlight = drawPathOnMap(flightPlan, flightID);
                //update tha the document need to react on click

            } else {
                //there is error
                showErrror();

            }

        },
        error: function () {
            showErrror();
        }
    });


}




function getEndTime(flight) {
    let date = new Date(flight.initial_location.date_time);
    for (let segment of flight.segments) {
        date.setSeconds(date.getSeconds() + segment.timespan_seconds);
    }
    return date;
}

