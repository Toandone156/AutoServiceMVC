let previewUpload = document.getElementById("preview-upload");
let previewImg = document.getElementById("preview-img");

function LoadFile(event) {
    previewImg.src = URL.createObjectURL(event.target.files[0]);
    previewUpload.classList.add("d-none");
    previewImg.onload = function() {
        URL.revokeObjectURL(output.src);
    }
};