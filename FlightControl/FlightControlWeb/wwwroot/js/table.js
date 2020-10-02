



function AddToTable(flight, tableID) {
    if (tableID == "myFlightTable") {
        $(myFlightTable).find('tbody').append('<tr id="' + flight.flight_id + '"><td class="first-td"><svg class="bi bi-x-square" id ="icon' + flight.flight_id + 'icon" width="1em" height="1em" onmouseover="" style="cursor: pointer;" viewBox="0 0 16 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M14 1H2a1 1 0 00-1 1v12a1 1 0 001 1h12a1 1 0 001-1V2a1 1 0 00-1-1zM2 0a2 2 0 00-2 2v12a2 2 0 002 2h12a2 2 0 002-2V2a2 2 0 00-2-2H2z" clip-rule="evenodd" /><path fill-rule="evenodd" d="M11.854 4.146a.5.5 0 010 .708l-7 7a.5.5 0 01-.708-.708l7-7a.5.5 0 01.708 0z" clip-rule="evenodd" /><path fill-rule="evenodd" d="M4.146 4.146a.5.5 0 000 .708l7 7a.5.5 0 00.708-.708l-7-7a.5.5 0 00-.708 0z" clip-rule="evenodd" /></svg></td><td class="second-td">' + flight.flight_id + '</td><td class="third-td">' + flight.company_name + '</td></tr >');
        document.getElementById("icon" + flight.flight_id + "icon").addEventListener("click", function (event) {
            deleteFlight(this);
            event.stopImmediatePropagation();
        }, false);
        document.getElementById(flight.flight_id).addEventListener("click", function () { clickFlight(this.id); }, false);

    }
    else {
        $(allFlightTable).find('tbody').append('<tr id="' + flight.flight_id + '"><td class="first-td">--</td><td class="second-td">' + flight.flight_id + '</td><td class="third-td">' + flight.company_name + '</td ></tr > ');
        document.getElementById(flight.flight_id).addEventListener("click", function () { clickFlight(this.id); }, false);

    }
}



function deleteRowInTable(idRow) {
    let row = document.getElementById(idRow);
    if (row == null) {
        return;
    }
    let i = row.rowIndex;
    const table = row.parentNode.parentNode;
    if (table.id == "myFlightTable" || table.id == "allFlightTable") {
        table.deleteRow(i);
    }
}


function findFlightById(flightID) {
    for (let flight of allFlightList) {
        if (flight.flight_id == flightID) {
            return flight;
        }
    }
}




