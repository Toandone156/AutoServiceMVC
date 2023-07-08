let discountTypes = document.querySelectorAll('.mydict input[type="radio"]');
let discountValueElements = document.querySelectorAll('.discount-value');
let discountPercentElements = document.querySelectorAll('.discount-percent');

discountTypes.forEach(el => {
    el.addEventListener("change", e => {
        selectType();
        deleteFieldsValue();
    });
})

function selectType() {
    let selectedType = document.querySelector('.mydict input[type="radio"]:checked');

    switch (selectedType.value) {
        case "1":
            discountPercentElements[0].querySelector("input").removeAttribute("data-rule-emptydiscountfields");
            discountPercentElements.forEach(el => {
                el.classList.add("d-none");
            })

            discountValueElements[0].querySelector("input").setAttribute("data-rule-emptydiscountfields", "true");
            discountValueElements.forEach(el => {
                el.classList.remove("d-none");
            })
            break;
        case "2":
            discountPercentElements[0].querySelector("input").setAttribute("data-rule-emptydiscountfields", "true");
            discountPercentElements.forEach(el => {
                el.classList.remove("d-none");
            })

            discountValueElements[0].querySelector("input").removeAttribute("data-rule-emptydiscountfields");
            discountValueElements.forEach(el => {
                el.classList.add("d-none");
            })
            break;
    }

    //addRequiredMark();
}

function deleteFieldsValue() {
    let deleteEls = new Set(
        [
            ...discountPercentElements,
            ...discountValueElements
        ]
    )

    deleteEls.forEach(el => {
        el.querySelector("input").value = ""
    })
}

selectType();

$.validator.addMethod("emptydiscountfields", function (value, element) {
    var discountValue = $('#DiscountValue').val();
    var discountPercentage = $('#DiscountPercentage').val();

    return discountValue !== "" || discountPercentage !== "";
}, "The field is required.");

$.validator.addMethod("checkenddate", function (value, element) {
    var startDate = $('#StartAt').val();
    var endDate = value;

    return startDate == "" || endDate == "" || (startDate < endDate)
}, "End date must later than Start date");