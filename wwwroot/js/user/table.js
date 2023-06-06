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

window.onclick = function (event) {
    // Fix cannot hide the cartModal
    if (
        event.target == modal ||
        event.target.classList.contains("no-gutters")
    ) {
        html5QrCode.stop();
    }
};