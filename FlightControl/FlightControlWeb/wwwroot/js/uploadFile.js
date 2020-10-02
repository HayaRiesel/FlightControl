

/////upload file
function intilizeUploadFile() {
    //the press buttom
    const fileSelect = document.getElementById("fileSelect");
    //the input of the json file
    const fileElem = document.getElementById("fileElem");

    fileSelect.addEventListener("click", function () {
        if (fileElem) {
            fileElem.click();
        }
    }, false);

    fileElem.addEventListener("change", handleFiles, false);

}

function handleFiles() {
    if (this.files[0] == null) {
        return;
    }
    const reader = new FileReader();
    reader.addEventListener('load', function (event){
        const text = event.target.result;
        postFlightPlan(text);
    });
    reader.readAsText(this.files[0]);

}

//check that all the fields in the json file
function checkJsonIsOk(textFlight) {
    const flight = JSON.parse(textFlight);
    if (!Object.prototype.hasOwnProperty.call(flight, "passengers") || !Object.prototype.hasOwnProperty.call(flight, "company_name") || !Object.prototype.hasOwnProperty.call(flight, "initial_location") || !Object.prototype.hasOwnProperty.call(flight, "segments")) {
        return false;
    }
    if (!Object.prototype.hasOwnProperty.call(flight.initial_location, "longitude") || !Object.prototype.hasOwnProperty.call(flight.initial_location, "latitude") || !Object.prototype.hasOwnProperty.call(flight.initial_location, "date_time")) {
        return false;
    }
    for (let seg of flight.segments) {
        if (!Object.prototype.hasOwnProperty.call(seg, "longitude") || !Object.prototype.hasOwnProperty.call(seg, "latitude") || !Object.prototype.hasOwnProperty.call(seg, "timespan_seconds")) {
            return false;
        }
    }
    return true;
}

//post the fligt plan
function postFlightPlan(textFlight) {
    $("body").css("cursor", "progress");
    if (checkJsonIsOk(textFlight) == false) {
        showErrror("can't upload the flight");
        return;
    }
    $(document).ready(function () {
        $.ajax({
            type: 'POST',
            url: 'api/flightPlan',
            data: textFlight,
            contentType: 'application/json',
            success: function () {
                deleteError(false);
            },
            error: function () {
                showErrror("can't upload the flight");
            }
        });
    });

}
