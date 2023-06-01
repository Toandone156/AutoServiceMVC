//Load product by category
function loadProduct(categoryId, thisitem) {

    var activeCategory = document.querySelector(".nav-link.active");
    activeCategory.classList.remove("active");

    thisitem.classList.add("active");

    var productRows = document.getElementsByClassName("product-item");

    for (var i = 0; i < productRows.length; i++) {
        var row = productRows[i];
        var category = row.getAttribute("data-category");

        if (categoryId == 0 || category == categoryId) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    }
}

function showToast(message) {
    Toastify({
        text: message,
        duration: 3000,
        style: {
            background: "linear-gradient(to right, #6f4e37, #b28451)",
        },
    }).showToast();
}