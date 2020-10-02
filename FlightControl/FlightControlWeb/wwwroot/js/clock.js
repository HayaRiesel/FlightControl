

function clock() {
    let time = new Date();
    time = time.toISOString();
    if (time.length == 24) {
        time = time.slice(0, 19) + time.slice(23);
    }
    if (time.length == 27) {
        time = time.slice(0, 22) + time.slice(26);
    }
    document.getElementById("datetime").innerHTML = time;
}
