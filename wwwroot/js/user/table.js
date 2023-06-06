var bookButton = document.querySelector(".bookbutton");
var scanButton = document.querySelector(".scanbutton");
var inputCode = document.querySelector(".tablecode");
var closeModal = document.getElementById("closeButton");
var modal = document.getElementById("scan_qr");

bookButton.addEventListener("click", e => {
    e.preventDefault();
    accessTableAjax(inputCode.value);
})

inputCode.addEventListener("keyup", e => {
    if (e.key === "Enter") {
        accessTableAjax(inputCode.value);
    }
})

let html5QrcodeScanner = new Html5QrcodeScanner(
    "reader",
    { fps: 10, qrbox: 250 });

function onScanSuccess(qrCodeMessage) {
    window.location.assign(qrCodeMessage);
}

function onScanFailure(error) {

}

scanButton.addEventListener("click", e => {
    html5QrcodeScanner.render(onScanSuccess, onScanFailure);
})

closeModal.addEventListener("click", e => {
    html5QrcodeScanner.clear();
})
