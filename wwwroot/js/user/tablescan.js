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

bookButton.style.display = "none";
scanButton.style.display = "block";

inputCode.addEventListener("focus", e => {
    bookButton.style.display = "block";
    scanButton.style.display = "none";
})

inputCode.addEventListener("blur", e => {
    bookButton.style.display = "none";
    scanButton.style.display = "block";
})

let html5QrcodeScanner = new Html5QrcodeScanner(
    "reader",
    { fps: 10, qrbox: 250 });

function onScanSuccess(decodedText, decodedResult) {
    window.location.assign(decodeText);
}

function onScanFailure(error) {

}

scanButton.addEventListener("click", e => {
    html5QrcodeScanner.render(onScanSuccess, onScanFailure);
})

closeModal.addEventListener("click", e => {
    html5QrcodeScanner.clear();
})
