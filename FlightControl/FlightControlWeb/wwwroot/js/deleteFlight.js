
function deleteFlight(rowIcon) {
    let flightID = rowIcon.parentNode.parentNode.id;
    let myUrl = 'api/Flights/' + flightID;
    $.ajax({
        type: 'DELETE',
        url: myUrl,
        success: function () {
            deleteError(false);
            deleteFlightOnTheView(flightID);
            currentIdFlight.set(flightID, "delete");
        },
        error: function () { // error callback 
            showErrror("can't delete the flight");
        }

    });
}

function deleteFlightOnTheView(flightID) {
    //check if the flight press
    if (crrentPresedFlight != null && crrentPresedFlight.rowID == flightID) {
        cancelClick(flightID);
    }

    currentIdFlight.delete(flightID);
    deleteRowInTable(flightID);
    deleteFlightMark(flightID);
}