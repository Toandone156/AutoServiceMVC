// Feedback Modal and button section
let feedbackModal = document.getElementById("feedbackModal");
let feedbackButton = document.getElementById("feedbackButton");
let sendFeedbackButtons = document.getElementsByClassName("send-feedback");
let cancelFeedbackButton = document.getElementsByClassName("cancel-feedback")[0];

// Modal content section
let productFeedbackImg = document.getElementById("product-feedback-img");
let productFeedbackName = document.getElementById("product-feedback-name");

for(let button of sendFeedbackButtons) {
    button.addEventListener("click", function() {
        DisplayFeedback();
    });
}
cancelFeedbackButton.onclick = HideFeedback;

// Feedback Modal function
function DisplayFeedback() {
    feedbackModal.style.display = "block";
}

function HideFeedback() {
    feedbackModal.style.display = "none";
}

function GiveFeedback(event) {
    DisplayFeedback();
    let productSection = event.target.parentElement.parentElement.parentElement.parentElement;
    
    console.log(productSection);
    let productName = productSection.getElementsByClassName("product-name")[0];
    let productImg = productSection.querySelector(".image-prod .img");
    productFeedbackName.innerText = productName.querySelector("h3").innerText;
    productFeedbackImg.style.backgroundImage = productImg.style.backgroundImage;
}