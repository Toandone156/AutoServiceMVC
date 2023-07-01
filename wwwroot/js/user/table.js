var bookButton = document.querySelector(".bookbutton");
var exitButton = document.querySelector(".exit-table");
var scanButton = document.querySelector(".scanbutton");
var inputCode = document.querySelector(".tablecode");
var closeModal = document.getElementById("closeButton");
var modal = document.getElementById("scan_qr");
var tableheader = document.getElementById("tableheader");

bookButton.addEventListener("click", e => {
    e.preventDefault();
    accessTableAjax(inputCode.value);
})

tableheader.addEventListener("click", opentable);

function closetable() {
    let tableform = document.querySelector(".table-form");
    tableform.classList.add("d-none");
    tableheader.removeEventListener("click", closetable);
    tableheader.addEventListener("click", opentable);
}

function opentable() {
    let tableform = document.querySelector(".table-form");
    tableform.classList.remove("d-none");
    tableheader.removeEventListener("click", opentable);
    tableheader.addEventListener("click", closetable);
}

inputCode.addEventListener("keyup", e => {
    if (e.key === "Enter") {
        accessTableAjax(inputCode.value);
    }
})

const html5QrCode = new Html5Qrcode("reader");
const config = { fps: 10, qrbox: 250 };

function onScanSuccess(qrCodeMessage) {
    window.location.assign(qrCodeMessage);
    html5QrCode.stop();
}

scanButton.addEventListener("click", e => {
    html5QrCode.start({ facingMode: "environment" }, config, onScanSuccess);
})

closeModal.addEventListener("click", e => {
    html5QrCode.stop();
})

window.addEventListener("click", event => {
    // Fix cannot hide the Modal
    if (
        event.target == modal ||
        event.target.classList.contains("no-gutters")
    ) {
        html5QrCode.stop();
    }
})