//Load product by category
function loadProduct(thisitem) {

    var activeCategory = document.querySelector(".nav-link.active");

    if (thisitem != null) {
        activeCategory.classList.remove("active");
        thisitem.classList.add("active");
        activeCategory = document.querySelector(".nav-link.active");
    }

    var searchbar = document.querySelector(".search-bar");

    var productRows = document.getElementsByClassName("product-item");
    var categoryId = activeCategory.getAttribute("data-categoryid");

    for (var i = 0; i < productRows.length; i++) {
        var row = productRows[i];
        var category = row.getAttribute("data-category");

        var productName = row.querySelector(".product-name").innerHTML;

        if ((categoryId == 0 || category == categoryId)
            && Diacritics.clean(productName).toLowerCase().includes(Diacritics.clean(searchbar.value).toLowerCase())) {
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
        close: true,
        style: {
            background: "linear-gradient(to right, #6f4e37, #b28451)",
        },
    }).showToast();
}