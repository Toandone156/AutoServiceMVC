// Feedback Modal and button section
let feedbackModal = document.getElementById("feedbackModal");
let feedbackButton = document.getElementById("feedbackButton");
let sendFeedbackButtons = document.getElementsByClassName("send-feedback");

// Modal content section
let productFeedbackImg = document.getElementById("product-feedback-img");
let productFeedbackName = document.getElementById("product-feedback-name");
let productIdInput = document.querySelector('input[name="ProductId"]');

for(let button of sendFeedbackButtons) {
    button.addEventListener("click", e => {
        let productSection = e.target.closest(".order-item");

        let productName = productSection.getElementsByClassName("product-name")[0];
        let productImg = productSection.querySelector(".image-prod .img");
        let productId = productSection.getAttribute("data-id");
        productFeedbackName.innerText = productName.querySelector("h3").innerText;
        productFeedbackImg.style.backgroundImage = productImg.style.backgroundImage;
        productIdInput.value = productId;
    });
}