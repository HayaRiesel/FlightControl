//handle errors
let timer = 0;
let generalError = false;
function showErrror(text) {
    //general problem of connect to server
    if (text == null) {
        text = "Error getting data from the server";
        generalError = true;
    }
    let element = document.getElementById("errorDetails");
    element.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" width="18px" height="18px"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg >' + '\xa0\xa0' + text;

    //the error i not general
    if (text != null) {
        //all error show for 5 sec. make the timer from the start
        clearTimeout(timer);
        timer = setTimeout(() => { deleteError(generalError) }, 5000);
    }

}

function deleteError(isGenaralError) {
    //check if there is another problem from the one that show right now
    if (generalError != isGenaralError) {
        //if it is another - dont delete it
        return;
    }
    //delete the error
    let element = document.getElementById("errorDetails");
    element.innerHTML = "<br>";
    //there is no general error right now
    generalError = false;
}

